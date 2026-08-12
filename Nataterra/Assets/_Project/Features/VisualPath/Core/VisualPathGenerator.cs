using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class VisualPathGenerator
    {
        private readonly VisualPathSettings settings;

        private readonly PortalPathSolver portalSolver;

        private readonly RoadPathProcessor roadProcessor;

        public VisualPathGenerator(
            VisualPathSettings settings)
        {
            this.settings = settings;

            portalSolver =
                new PortalPathSolver(settings);

            roadProcessor =
                new RoadPathProcessor(settings);
        }

        public VisualPathResult Generate(
            VisualPathRequest request)
        {
            // ---------------------------------------------------------
            // Validate
            // ---------------------------------------------------------

            if (!Validate(
                    request,
                    out string error))
            {
                return VisualPathResult.Failed(error);
            }

            // ---------------------------------------------------------
            // Create portals
            // ---------------------------------------------------------

            List<PathPortal> portals =
                CreatePortals(
                    request.Corridor);

            if (portals == null)
            {
                return VisualPathResult.Failed(
                    "Failed to create path portals.");
            }

            // ---------------------------------------------------------
            // Generate portal path
            // ---------------------------------------------------------

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

            // ---------------------------------------------------------
            // Optimise path
            // ---------------------------------------------------------

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

            // ---------------------------------------------------------
            // Try road
            // ---------------------------------------------------------

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
                        List<Vector3> roadFinalPath =
                            PathSmoother.SmoothTurns(
                                roadResult.Points,
                                settings.SmoothingRatio,
                                settings.SmoothingSamples);

                        GuaranteeEndpoints(
                            roadFinalPath,
                            request.Start,
                            request.End);

                        return VisualPathResult.Create(
                            solution.GetPositions(),
                            optimizedPath,
                            roadFinalPath,
                            true);
                    }
                }
            }

            // ---------------------------------------------------------
            // Normal path
            // ---------------------------------------------------------

            List<Vector3> finalPath =
                PathSmoother.SmoothTurns(
                    optimizedPath,
                    settings.SmoothingRatio,
                    settings.SmoothingSamples);

            GuaranteeEndpoints(
                finalPath,
                request.Start,
                request.End);

            // ---------------------------------------------------------
            // Return
            // ---------------------------------------------------------

            return VisualPathResult.Create(
                solution.GetPositions(),
                optimizedPath,
                finalPath,
                false);
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

                portals.Add(portal);
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

            path[0] = start;

            if (path.Count > 1)
            {
                path[path.Count - 1] = end;
            }
        }
    }
}