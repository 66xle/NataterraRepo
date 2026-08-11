
using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public static class StraightPathOptimizer
    {
        public static List<Vector3> RemoveRedundantPoints(IReadOnlyList<Vector3> points, IReadOnlyList<PathPortal> portals, float angleTolerance)
        {
            if (points == null || points.Count <= 2)
                return new List<Vector3>(points);

            var result = new List<Vector3>();
            int currentPoint = 0;

            result.Add(points[0]);

            while (currentPoint < points.Count - 1)
            {
                int furthestValid = currentPoint + 1;

                for (int candidate = currentPoint + 2; candidate < points.Count; candidate++)
                {
                    if (!CanSkipTo(currentPoint, candidate, points, portals, angleTolerance))
                        break;

                    furthestValid = candidate;
                }

                result.Add(points[furthestValid]);
                currentPoint = furthestValid;
            }

            return result;
        }

        private static bool CanSkipTo(int fromIndex, int toIndex, IReadOnlyList<Vector3> points, IReadOnlyList<PathPortal> portals, float angleTolerance)
        {
            if (fromIndex < 0 || toIndex >= points.Count || fromIndex >= toIndex)
                return false;

            Vector3 from = points[fromIndex];
            Vector3 to = points[toIndex];

            int firstPortal = fromIndex;
            int lastPortalExclusive = toIndex - 1;

            for (int portalIndex = firstPortal; portalIndex < lastPortalExclusive; portalIndex++)
            {
                if (portalIndex < 0 || portalIndex >= portals.Count)
                    return false;

                PathPortal portal = portals[portalIndex];

                if (!PathCrossesPortal(from, to, portal))
                    return false;
            }

            if (fromIndex > 0 && toIndex < points.Count - 1)
            {
                Vector3 previous = points[fromIndex - 1];
                Vector3 next = points[toIndex + 1];

                Vector3 direction = (to - from).normalized;
                Vector3 previousDirection = (from - previous).normalized;
                Vector3 nextDirection = (next - to).normalized;

                if (Vector3.Angle(previousDirection, direction) > angleTolerance)
                    return false;

                if (Vector3.Angle(direction, nextDirection) > angleTolerance)
                    return false;
            }

            return true;
        }

        private static bool PathCrossesPortal(Vector3 from, Vector3 to, PathPortal portal)
        {
            if (PathMath.TrySegmentIntersectionXZ(from, to, portal.PointA, portal.PointB, out _))
                return true;

            float distance = PathMath.SegmentDistanceXZ(from, to, portal.PointA, portal.PointB, out _, out _);

            return distance <= 0.01f;
        }
    }
}
