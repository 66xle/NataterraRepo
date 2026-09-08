using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VisualPath
{
    public sealed class VisualPathGenerator
    {
        private readonly VisualPathSettings settings;

        private readonly PortalPathSolver portalSolver;

        private readonly RoadPathProcessor roadProcessor;

        private readonly NavMeshPathProcessor navMeshProcessor;

        public VisualPathGenerator(
            VisualPathSettings settings,
            NavMeshData navMeshData = null)
        {
            this.settings =
                settings;

            if (navMeshData != null)
            {
                this.settings.NavMeshData =
                    navMeshData;
            }

            portalSolver =
                new PortalPathSolver(
                    settings);

            roadProcessor =
                new RoadPathProcessor(
                    settings);

            navMeshProcessor =
                new NavMeshPathProcessor(
                    settings);
        }

        public VisualPathResult Generate(
            VisualPathRequest request)
        {
            // =========================================================
            // VALIDATE
            // =========================================================

            if (!Validate(
                    request,
                    out string error))
            {
                return VisualPathResult.Failed(
                    error);
            }

            // =========================================================
            // CREATE PORTALS
            // =========================================================

            List<PathPortal> portals =
                CreatePortals(
                    request.Corridor);

            if (portals == null)
            {
                return VisualPathResult.Failed(
                    "Failed to create path portals.");
            }

            // =========================================================
            // GENERATE PORTAL PATH
            // =========================================================

            PortalPathSolution solution =
                portalSolver.Solve(
                    request.Start,
                    request.End,
                    portals);

            if (solution == null ||
                solution.Points == null ||
                solution.Points.Count < 2)
            {
                return VisualPathResult.Failed(
                    "Failed to generate visual path.");
            }

            // =========================================================
            // OPTIMIZE
            // =========================================================

            List<Vector3> optimizedPath;

            List<int> portalIndices;

            StraightPathOptimizer.RemoveRedundantPoints(
                solution.Points,
                settings.StraightAngleTolerance,
                out optimizedPath,
                out portalIndices);

            if (optimizedPath == null ||
                optimizedPath.Count < 2)
            {
                return VisualPathResult.Failed(
                    "Failed to optimize visual path.");
            }

            // =========================================================
            // ROAD PATH
            // =========================================================

            List<Vector3> selectedPath =
                null;

            bool usesRoad =
                false;

            if (settings.PathPriority ==
                PathPriorityMode.PreferRoads)
            {
                VisualRoad road =
                    roadProcessor.FindBestRoad(
                        request.Corridor,
                        request.Roads);

                if (road != null)
                {
                    bool roadAvailable =
                        roadProcessor.TryBuildRoadPath(
                            road,
                            request.Start,
                            request.End,
                            request.Corridor,
                            optimizedPath,
                            portalIndices,
                            portals,
                            out RoadPathResult roadResult);

                    if (roadAvailable &&
                        roadResult != null &&
                        roadResult.Points != null &&
                        roadResult.Points.Count >= 2)
                    {
                        selectedPath =
                            new List<Vector3>(
                                roadResult.Points);

                        usesRoad =
                            true;
                    }
                }
            }

            if (selectedPath == null)
            {
                selectedPath =
                    new List<Vector3>(
                        optimizedPath);
            }

            // =========================================================
            // ENDPOINTS
            // =========================================================

            GuaranteeEndpoints(
                selectedPath,
                request.Start,
                request.End);

            // =========================================================
            // NAVMESH VALIDATION / REPAIR
            // =========================================================

            if (settings.UseNavMesh)
            {
                bool repaired =
                    navMeshProcessor.TryValidateAndRepairPath(
                        selectedPath,
                        request.Corridor,
                        out List<Vector3> navMeshPath,
                        out bool usedFallback);

                if (!repaired ||
                    navMeshPath == null ||
                    navMeshPath.Count < 2)
                {
                    return VisualPathResult.Failed(
                        "Generated path is not walkable on the NavMesh and no valid corridor-constrained fallback path could be found.");
                }

                selectedPath =
                    navMeshPath;

                if (usedFallback)
                {
                    usesRoad =
                        false;
                }
            }

            // =========================================================
            // CORRIDOR VALIDATION
            // =========================================================

            if (!CorridorPathValidator.IsPathInsideCorridor(
                    selectedPath,
                    request.Corridor))
            {
                return VisualPathResult.Failed(
                    "Generated path leaves the supplied corridor.");
            }

            // =========================================================
            // SMOOTH
            // =========================================================

            List<Vector3> finalPath =
                PathSmoother.SmoothTurns(
                    selectedPath,
                    settings.SmoothingRatio,
                    settings.SmoothingSamples);

            GuaranteeEndpoints(
                finalPath,
                request.Start,
                request.End);

            // =========================================================
            // MATCH SMOOTHED PATH TO NAVMESH HEIGHT
            // =========================================================

            if (settings.UseNavMesh)
            {
                if (!ProjectPathToNavMeshHeight(
                        finalPath))
                {
                    finalPath = null;
                }
            }

            // =========================================================
            // VALIDATE FINAL PATH
            // =========================================================

            bool finalPathValid =
                finalPath != null &&
                CorridorPathValidator.IsPathInsideCorridor(
                    finalPath,
                    request.Corridor);

            if (settings.UseNavMesh &&
                settings.ValidateSmoothedPath)
            {
                finalPathValid =
                    finalPathValid &&
                    IsNavMeshPathValid(
                        finalPath);
            }

            // =========================================================
            // FALLBACK TO UNSMOOTHED PATH
            // =========================================================

            if (!finalPathValid)
            {
                finalPath =
                    new List<Vector3>(
                        selectedPath);

                GuaranteeEndpoints(
                    finalPath,
                    request.Start,
                    request.End);
            }

            // =========================================================
            // RESULT
            // =========================================================

            return VisualPathResult.Create(
                solution.GetPositions(),
                selectedPath,
                finalPath,
                usesRoad);
        }

        // =============================================================
        // NAVMESH VALIDATION
        // =============================================================

        private bool IsNavMeshPathValid(
            IReadOnlyList<Vector3> path)
        {
            if (path == null ||
                path.Count < 2)
            {
                return false;
            }

            // Validate every point.
            for (int i = 0;
                 i < path.Count;
                 i++)
            {
                if (!TrySampleNavMesh(
                        path[i],
                        out _))
                {
                    return false;
                }
            }

            // Validate every segment.
            for (int i = 0;
                 i < path.Count - 1;
                 i++)
            {
                if (!TrySampleNavMesh(
                        path[i],
                        out NavMeshHit startHit))
                {
                    return false;
                }

                if (!TrySampleNavMesh(
                        path[i + 1],
                        out NavMeshHit endHit))
                {
                    return false;
                }

                if (NavMesh.Raycast(
                        startHit.position,
                        endHit.position,
                        out _,
                        settings.NavMeshAreaMask))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Samples the NavMesh using an XZ distance and an independent
        /// Y tolerance.
        /// </summary>
        private bool TrySampleNavMesh(
            Vector3 position,
            out NavMeshHit hit)
        {
            hit = default;

            float xzDistance =
                Mathf.Max(
                    0.01f,
                    settings.NavMeshSampleDistance);

            float heightTolerance =
                Mathf.Max(
                    0.01f,
                    settings.NavMeshHeightTolerance);

            float searchRadius =
                Mathf.Sqrt(
                    xzDistance * xzDistance +
                    heightTolerance * heightTolerance);

            if (!NavMesh.SamplePosition(
                    position,
                    out NavMeshHit candidate,
                    searchRadius,
                    settings.NavMeshAreaMask))
            {
                return false;
            }

            float actualXZDistance =
                PathMath.DistanceXZ(
                    position,
                    candidate.position);

            float actualHeightDifference =
                Mathf.Abs(
                    position.y -
                    candidate.position.y);

            if (actualXZDistance >
                xzDistance)
            {
                return false;
            }

            if (actualHeightDifference >
                heightTolerance)
            {
                return false;
            }

            hit =
                candidate;

            return true;
        }

        // =============================================================
        // NAVMESH HEIGHT
        // =============================================================

        /// <summary>
        /// Projects the path vertically onto the NavMesh.
        ///
        /// X and Z are preserved.
        /// Only Y is changed.
        /// </summary>
        private bool ProjectPathToNavMeshHeight(
            List<Vector3> path)
        {
            if (path == null ||
                path.Count == 0)
            {
                return false;
            }

            for (int i = 0;
                 i < path.Count;
                 i++)
            {
                Vector3 point =
                    path[i];

                if (!TrySampleNavMesh(
                        point,
                        out NavMeshHit hit))
                {
                    return false;
                }

                // Preserve the generated X/Z position.
                point.y =
                    hit.position.y;

                path[i] =
                    point;
            }

            return true;
        }

        // =============================================================
        // PORTALS
        // =============================================================

        private List<PathPortal> CreatePortals(
            IReadOnlyList<VisualHex> corridor)
        {
            if (corridor == null ||
                corridor.Count < 2)
            {
                return new List<PathPortal>();
            }

            var portals =
                new List<PathPortal>(
                    corridor.Count - 1);

            for (int i = 0;
                 i < corridor.Count - 1;
                 i++)
            {
                if (!HexPathGeometry.TryCreatePortal(
                        corridor[i],
                        corridor[i + 1],
                        settings,
                        out PathPortal portal))
                {
                    return null;
                }

                portals.Add(
                    portal);
            }

            return portals;
        }

        // =============================================================
        // VALIDATION
        // =============================================================

        private bool Validate(
            VisualPathRequest request,
            out string error)
        {
            if (request == null)
            {
                error =
                    "Request is null.";

                return false;
            }

            if (request.Corridor == null ||
                request.Corridor.Count == 0)
            {
                error =
                    "Corridor is empty.";

                return false;
            }

            if (request.Corridor.Count == 1)
            {
                error =
                    "Corridor contains only one hex.";

                return false;
            }

            error = null;

            return true;
        }

        // =============================================================
        // ENDPOINTS
        // =============================================================

        private void GuaranteeEndpoints(
            List<Vector3> path,
            Vector3 start,
            Vector3 end)
        {
            if (path == null ||
                path.Count == 0)
            {
                return;
            }

            path[0] =
                start;

            if (path.Count > 1)
            {
                path[path.Count - 1] =
                    end;
            }
        }
    }
}