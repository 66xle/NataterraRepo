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
        private readonly PostRepairPathOptimizer postRepairOptimizer;

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

            postRepairOptimizer =
                new PostRepairPathOptimizer(
                    settings);
        }

        public VisualPathResult Generate(
            VisualPathRequest request)
        {
            return Generate(
                request,
                out _);
        }

        public VisualPathResult Generate(
            VisualPathRequest request,
            out List<NavMeshBlockerRequest> blockerRequests)
        {
            blockerRequests =
                null;

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
                        request.AllHexes,
                        out List<Vector3> navMeshPath,
                        out bool usedFallback,
                        out blockerRequests);

                if (!repaired)
                {
                    if (blockerRequests != null &&
                        blockerRequests.Count > 0)
                    {
                        return VisualPathResult.Failed(
                            "NavMesh fallback left the corridor and requires temporary corridor blockers.");
                    }

                    return VisualPathResult.Failed(
                        "Generated path is not walkable on the NavMesh and no valid corridor-constrained fallback path could be found.");
                }

                if (navMeshPath == null ||
                    navMeshPath.Count < 2)
                {
                    return VisualPathResult.Failed(
                        "NavMesh repair produced an invalid path.");
                }

                selectedPath =
                    navMeshPath;

                // -----------------------------------------------------
                // POST-REPAIR OPTIMIZATION
                // -----------------------------------------------------

                if (usedFallback)
                {
                    if (postRepairOptimizer.TryOptimize(
                            selectedPath,
                            request.Corridor,
                            out List<Vector3> postRepairPath))
                    {
                        selectedPath =
                            postRepairPath;
                    }

                    // A fallback may have replaced part of a road
                    // route, so do not continue reporting this as a
                    // complete road path.
                    usesRoad =
                        false;

                    GuaranteeEndpoints(
                        selectedPath,
                        request.Start,
                        request.End);
                }

                // =====================================================
                // NAVMESH EDGE AVOIDANCE
                //
                // Apply this to EVERY selected path, regardless of
                // whether NavMesh repair was required.
                //
                // This happens after all repair and post-repair
                // optimization so we only avoid edges on the complete
                // current path.
                // =====================================================

                if (!navMeshProcessor.TryApplyEdgeAvoidance(
                        selectedPath,
                        request.Corridor))
                {
                    return VisualPathResult.Failed(
                        "NavMesh edge avoidance produced a path that is no longer valid.");
                }

                GuaranteeEndpoints(
                    selectedPath,
                    request.Start,
                    request.End);
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
            // FINAL NAVMESH EDGE AVOIDANCE
            //
            // Smoothing can move interior points toward NavMesh edges.
            //
            // Therefore the actual final path gets its own edge
            // avoidance pass.
            //
            // Because ApplyEdgeAvoidance only moves points that are
            // within the configured edge-avoidance distance, points
            // already moved away during the selected-path pass will
            // generally remain unchanged here.
            // =========================================================

            if (settings.UseNavMesh &&
                finalPath != null)
            {
                if (!navMeshProcessor.TryApplyEdgeAvoidance(
                        finalPath,
                        request.Corridor))
                {
                    finalPath = null;
                }

                if (finalPath != null)
                {
                    GuaranteeEndpoints(
                        finalPath,
                        request.Start,
                        request.End);
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
            //
            // IMPORTANT:
            //
            // selectedPath has ALREADY gone through edge avoidance.
            // Therefore falling back to it still gives us an
            // edge-avoided final path.
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
        //
        // A segment is valid only when CalculatePath produces exactly
        // two corners.
        //
        // NavMesh.Raycast is intentionally not used.
        // =============================================================

        private bool IsNavMeshPathValid(
            IReadOnlyList<Vector3> path)
        {
            if (path == null ||
                path.Count < 2)
            {
                return false;
            }

            // ---------------------------------------------------------
            // POINT VALIDATION
            // ---------------------------------------------------------

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

            // ---------------------------------------------------------
            // SEGMENT VALIDATION
            // ---------------------------------------------------------

            for (int i = 0;
                 i < path.Count - 1;
                 i++)
            {
                NavMeshPath navMeshPath =
                    new NavMeshPath();

                bool calculated =
                    NavMesh.CalculatePath(
                        path[i],
                        path[i + 1],
                        settings.NavMeshAreaMask,
                        navMeshPath);

                if (!calculated)
                {
                    return false;
                }

                if (navMeshPath.status !=
                    NavMeshPathStatus.PathComplete)
                {
                    return false;
                }

                // Exactly two corners means the segment can be
                // travelled directly.
                if (navMeshPath.corners == null ||
                    navMeshPath.corners.Length != 2)
                {
                    return false;
                }
            }

            return true;
        }

        // =============================================================
        // NAVMESH SAMPLING
        // =============================================================

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
                    xzDistance *
                    xzDistance +
                    heightTolerance *
                    heightTolerance);

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

            if (settings.UseNavMesh &&
                (request.AllHexes == null ||
                 request.AllHexes.Count == 0))
            {
                error =
                    "NavMesh validation requires the full map hex collection in VisualPathRequest.AllHexes.";

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