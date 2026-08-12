using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public static class StraightPathOptimizer
    {
        public static void RemoveRedundantPoints(
            IReadOnlyList<PortalPathPoint> points,
            float angleTolerance,
            out List<Vector3> result,
            out List<int> portalIndices)
        {
            result =
                new List<Vector3>();

            portalIndices =
                new List<int>();

            if (points == null ||
                points.Count == 0)
            {
                return;
            }

            /*
             * Always preserve the first point.
             */
            AddPoint(
                result,
                portalIndices,
                points[0]);

            if (points.Count == 1)
            {
                return;
            }

            /*
             * The anchor is the last point that was actually
             * included in the visual path.
             */
            int anchorIndex = 0;

            for (int currentIndex = 1;
                 currentIndex < points.Count - 1;
                 currentIndex++)
            {
                int nextIndex =
                    currentIndex + 1;

                PortalPathPoint current =
                    points[currentIndex];

                /*
                 * Portal points are important metadata even when
                 * their position is visually redundant.
                 *
                 * Do not use the portal point as a reason to
                 * permanently discard its identity.
                 */
                bool isPortal =
                    current.PortalIndex >= 0;

                bool straight =
                    IsEffectivelyStraight(
                        points[anchorIndex].Position,
                        current.Position,
                        points[nextIndex].Position,
                        angleTolerance);

                /*
                 * If this point creates a meaningful turn,
                 * keep it.
                 */
                if (!straight)
                {
                    AddPoint(
                        result,
                        portalIndices,
                        current);

                    anchorIndex =
                        currentIndex;

                    continue;
                }

                /*
                 * The point is redundant geometrically.
                 *
                 * If it is a portal, we don't add its position
                 * to the visual path, but its portal index is
                 * still preserved separately.
                 *
                 * This is handled after the visual optimisation.
                 */
                if (isPortal)
                {
                    continue;
                }
            }

            /*
             * Always preserve the final point.
             */
            AddPoint(
                result,
                portalIndices,
                points[points.Count - 1]);
        }

        // =============================================================
        // STRAIGHT TEST
        // =============================================================

        private static bool IsEffectivelyStraight(
            Vector3 previous,
            Vector3 current,
            Vector3 next,
            float angleTolerance)
        {
            Vector3 directionA =
                current - previous;

            Vector3 directionB =
                next - current;

            /*
             * Only compare movement on the XZ plane.
             */
            directionA.y = 0f;
            directionB.y = 0f;

            if (directionA.sqrMagnitude <=
                Mathf.Epsilon ||
                directionB.sqrMagnitude <=
                Mathf.Epsilon)
            {
                return true;
            }

            float angle =
                Vector3.Angle(
                    directionA,
                    directionB);

            /*
             * Also handle the case where the directions are
             * essentially identical.
             */
            return angle <=
                   Mathf.Max(
                       0f,
                       angleTolerance);
        }

        // =============================================================
        // ADD POINT
        // =============================================================

        private static void AddPoint(
            List<Vector3> result,
            List<int> portalIndices,
            PortalPathPoint point)
        {
            /*
             * Avoid duplicate positions.
             */
            if (result.Count > 0 &&
                PathMath.DistanceXZ(
                    result[result.Count - 1],
                    point.Position) <=
                0.001f)
            {
                /*
                 * If the duplicate point represents a portal,
                 * preserve its portal index.
                 */
                if (point.PortalIndex >= 0)
                {
                    int existingIndex =
                        portalIndices[
                            portalIndices.Count - 1];

                    /*
                     * Don't overwrite another valid portal index.
                     */
                    if (existingIndex < 0)
                    {
                        portalIndices[
                            portalIndices.Count - 1] =
                            point.PortalIndex;
                    }
                }

                return;
            }

            result.Add(
                point.Position);

            portalIndices.Add(
                point.PortalIndex);
        }
    }
}