using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class VisualPathGenerator
    {
        private readonly VisualPathSettings settings;
        private readonly PortalPathSolver portalSolver;
        private readonly RoadPathProcessor roadProcessor;

        public VisualPathGenerator(VisualPathSettings settings)
        {
            this.settings = settings;

            portalSolver = new PortalPathSolver(settings);

            roadProcessor = new RoadPathProcessor(settings);
        }

        public VisualPathResult Generate(VisualPathRequest request)
        {
            if (!Validate(request, out string error))
            {
                return VisualPathResult.Failed(error);
            }

            List<PathPortal> portals = CreatePortals(request.Corridor);

            if (portals == null)
            {
                return VisualPathResult.Failed("Failed to create path portals.");
            }

            VisualRoad road = roadProcessor.FindBestRoad(portals, request.Roads);

            bool roadAvailable = roadProcessor.TryBuildRoadPath(road, request.Start, request.End, portals, out List<Vector3> roadPath);

            List<Vector3> rawPath = portalSolver.Solve(request.Start, request.End, portals);
            List<Vector3> optimizedPath = StraightPathOptimizer.RemoveRedundantPoints(rawPath, portals, settings.StraightAngleTolerance);

            bool useRoad = ShouldUseRoad(roadAvailable);
            List<Vector3> selectedPath = useRoad ? roadPath : optimizedPath;


            List<Vector3> finalPath = PathSmoother.SmoothTurns(selectedPath, settings.SmoothingRatio);

            // Guarantee exact start/end positions.
            if (finalPath.Count > 0)
            {
                finalPath[0] = request.Start;
                finalPath[finalPath.Count - 1] = request.End;
            }

            return VisualPathResult.Create(rawPath, optimizedPath, finalPath, useRoad);
        }

        private bool ShouldUseRoad(bool roadAvailable)
        {
            if (!roadAvailable)
                return false;

            return settings.PathPriority == PathPriorityMode.RoadsFirst;
        }

        private List<PathPortal> CreatePortals(IReadOnlyList<VisualHex> corridor)
        {
            var portals = new List<PathPortal>(corridor.Count - 1);

            for (int i = 0; i < corridor.Count - 1; i++)
            {
                if (!HexPathGeometry.TryCreatePortal(corridor[i], corridor[i + 1], settings, out PathPortal portal))
                {
                    return null;
                }

                portals.Add(portal);
            }

            return portals;
        }

        private bool Validate(VisualPathRequest request, out string error)
        {
            if (request == null)
            {
                error = "Request is null.";

                return false;
            }

            if (request.Corridor == null ||
                request.Corridor.Count == 0)
            {
                error = "Corridor is empty.";

                return false;
            }

            if (request.Corridor.Count == 1)
            {
                error = null;
                return true;
            }

            error = null;

            return true;
        }
    }
}