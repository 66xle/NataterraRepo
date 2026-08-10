using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class RoadPathProcessor
    {
        private readonly VisualPathSettings settings;

        public RoadPathProcessor(VisualPathSettings settings)
        {
            this.settings = settings;
        }

        public VisualRoad FindBestRoad(IReadOnlyList<PathPortal> portals, IReadOnlyList<VisualRoad> roads)
        {
            if (roads == null || roads.Count == 0)
            {
                return null;
            }

            VisualRoad bestRoad = null;

            foreach (VisualRoad road in roads)
            {
                if (!CoversCorridor(road, portals))
                {
                    continue;
                }

                if (bestRoad == null || road.Priority > bestRoad.Priority)
                {
                    bestRoad = road;
                }
            }

            return bestRoad;
        }

        public bool TryBuildRoadPath(VisualRoad road, Vector3 start, Vector3 end, IReadOnlyList<PathPortal> portals, out List<Vector3> path)
        {
            path = new List<Vector3>();

            if (road == null || road.Points == null || road.Points.Count < 2)
            {
                return false;
            }

            var crossings = new List<Vector3>(portals.Count);

            foreach (PathPortal portal in portals)
            {
                if (!TryFindRoadPortalPoint(road, portal, out Vector3 crossing))
                {
                    return false;
                }

                crossings.Add(crossing);
            }

            path.Add(start);

            foreach (Vector3 crossing in crossings)
            {
                path.Add(crossing);
            }

            path.Add(end);

            return true;
        }

        private bool CoversCorridor(VisualRoad road, IReadOnlyList<PathPortal> portals)
        {
            foreach (PathPortal portal in portals)
            {
                if (!road.HexIds.Contains(portal.HexA.Id) || !road.HexIds.Contains(portal.HexB.Id))
                {
                    return false;
                }
            }

            return true;
        }

        private bool TryFindRoadPortalPoint(VisualRoad road, PathPortal portal, out Vector3 bestPoint)
        {
            float bestDistance = float.MaxValue;

            bestPoint = default;

            for (int i = 0; i < road.Points.Count - 1; i++)
            {
                Vector3 roadA = road.Points[i];

                Vector3 roadB = road.Points[i + 1];

                float distance = PathMath.SegmentDistanceXZ(roadA, roadB, portal.PointA, portal.PointB, out _, out Vector3 portalPoint);

                if (distance < bestDistance)
                {
                    bestDistance = distance;

                    bestPoint = portalPoint;
                }
            }

            return bestDistance <= settings.RoadPortalTolerance;
        }
    }
}