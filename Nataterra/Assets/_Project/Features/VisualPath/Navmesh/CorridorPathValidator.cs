using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public static class CorridorPathValidator
    {
        private const float Tolerance = 0.001f;

        // =============================================================
        // PATH VALIDATION
        // =============================================================

        public static bool IsPathInsideCorridor(
            IReadOnlyList<Vector3> path,
            IReadOnlyList<VisualHex> corridor)
        {
            if (path == null ||
                path.Count < 2 ||
                corridor == null ||
                corridor.Count == 0)
            {
                return false;
            }

            // ---------------------------------------------------------
            // Validate every point.
            // ---------------------------------------------------------

            for (int i = 0;
                 i < path.Count;
                 i++)
            {
                if (!IsPointInsideCorridor(
                        path[i],
                        corridor))
                {
                    return false;
                }
            }

            // ---------------------------------------------------------
            // Validate every segment.
            // ---------------------------------------------------------

            for (int i = 0;
                 i < path.Count - 1;
                 i++)
            {
                if (!IsSegmentInsideCorridor(
                        path[i],
                        path[i + 1],
                        corridor))
                {
                    return false;
                }
            }

            return true;
        }

        // =============================================================
        // CORRIDOR POINT
        // =============================================================

        public static bool IsPointInsideCorridor(
            Vector3 point,
            IReadOnlyList<VisualHex> corridor)
        {
            if (corridor == null)
            {
                return false;
            }

            for (int i = 0;
                 i < corridor.Count;
                 i++)
            {
                if (IsPointInsideHex(
                        point,
                        corridor[i]))
                {
                    return true;
                }
            }

            return false;
        }

        // =============================================================
        // PUBLIC HEX VALIDATION
        // =============================================================

        /// <summary>
        /// Determines whether a point is inside the supplied hex.
        ///
        /// The test is performed in X/Z only. Y is ignored.
        /// Points on the hex boundary are considered inside.
        /// </summary>
        public static bool IsPointInsideHexPublic(
            Vector3 point,
            VisualHex hex)
        {
            return IsPointInsideHex(
                point,
                hex);
        }

        // =============================================================
        // SEGMENT VALIDATION
        // =============================================================

        public static bool IsSegmentInsideCorridor(
            Vector3 start,
            Vector3 end,
            IReadOnlyList<VisualHex> corridor)
        {
            if (corridor == null ||
                corridor.Count == 0)
            {
                return false;
            }

            // ---------------------------------------------------------
            // Both endpoints must be inside the corridor.
            // ---------------------------------------------------------

            if (!IsPointInsideCorridor(
                    start,
                    corridor))
            {
                return false;
            }

            if (!IsPointInsideCorridor(
                    end,
                    corridor))
            {
                return false;
            }

            // ---------------------------------------------------------
            // Find all intersections between the segment and corridor
            // hex boundaries.
            // ---------------------------------------------------------

            var intersections =
                new List<float>();

            intersections.Add(0f);
            intersections.Add(1f);

            for (int hexIndex = 0;
                 hexIndex < corridor.Count;
                 hexIndex++)
            {
                VisualHex hex =
                    corridor[hexIndex];

                if (hex.Corners == null ||
                    hex.Corners.Length < 3)
                {
                    continue;
                }

                for (int edgeIndex = 0;
                     edgeIndex < hex.Corners.Length;
                     edgeIndex++)
                {
                    Vector3 a =
                        hex.Corners[edgeIndex];

                    Vector3 b =
                        hex.Corners[
                            (edgeIndex + 1) %
                            hex.Corners.Length];

                    if (TryGetSegmentIntersectionT(
                            start,
                            end,
                            a,
                            b,
                            out float t))
                    {
                        intersections.Add(t);
                    }
                }
            }

            intersections.Sort();

            // ---------------------------------------------------------
            // Test the midpoint of every interval between intersections.
            //
            // If any interval lies outside every corridor hex, the
            // segment leaves the corridor.
            // ---------------------------------------------------------

            for (int i = 0;
                 i < intersections.Count - 1;
                 i++)
            {
                float tA =
                    intersections[i];

                float tB =
                    intersections[i + 1];

                if (tB - tA <= Tolerance)
                {
                    continue;
                }

                float midpointT =
                    (tA + tB) * 0.5f;

                Vector3 midpoint =
                    Vector3.Lerp(
                        start,
                        end,
                        midpointT);

                if (!IsPointInsideCorridor(
                        midpoint,
                        corridor))
                {
                    return false;
                }
            }

            return true;
        }

        // =============================================================
        // HEX POINT TEST
        // =============================================================

        private static bool IsPointInsideHex(
            Vector3 point,
            VisualHex hex)
        {
            if (hex.Corners == null ||
                hex.Corners.Length < 3)
            {
                return false;
            }

            bool inside = false;

            float x =
                point.x;

            float z =
                point.z;

            for (int i = 0;
                 i < hex.Corners.Length;
                 i++)
            {
                Vector3 a =
                    hex.Corners[i];

                Vector3 b =
                    hex.Corners[
                        (i + 1) %
                        hex.Corners.Length];

                // -----------------------------------------------------
                // Points directly on an edge are considered inside.
                // -----------------------------------------------------

                if (DistanceToSegmentXZ(
                        point,
                        a,
                        b) <= Tolerance)
                {
                    return true;
                }

                // -----------------------------------------------------
                // Standard X/Z ray-casting test.
                // -----------------------------------------------------

                bool intersects =
                    ((a.z > z) != (b.z > z)) &&
                    (x <
                     (b.x - a.x) *
                     (z - a.z) /
                     (b.z - a.z) +
                     a.x);

                if (intersects)
                {
                    inside =
                        !inside;
                }
            }

            return inside;
        }

        // =============================================================
        // DISTANCE TO EDGE
        // =============================================================

        private static float DistanceToSegmentXZ(
            Vector3 point,
            Vector3 a,
            Vector3 b)
        {
            Vector3 direction =
                b - a;

            direction.y = 0f;

            Vector3 relative =
                point - a;

            relative.y = 0f;

            float lengthSq =
                direction.sqrMagnitude;

            if (lengthSq <=
                Mathf.Epsilon)
            {
                return relative.magnitude;
            }

            float t =
                Mathf.Clamp01(
                    Vector3.Dot(
                        relative,
                        direction)
                    /
                    lengthSq);

            Vector3 projection =
                a +
                direction * t;

            return PathMath.DistanceXZ(
                point,
                projection);
        }

        // =============================================================
        // SEGMENT INTERSECTION
        // =============================================================

        private static bool TryGetSegmentIntersectionT(
            Vector3 a,
            Vector3 b,
            Vector3 c,
            Vector3 d,
            out float t)
        {
            float denominator =
                (b.x - a.x) *
                (d.z - c.z)
                -
                (b.z - a.z) *
                (d.x - c.x);

            if (Mathf.Abs(
                    denominator) <
                0.00001f)
            {
                t = 0f;

                return false;
            }

            float u =
                ((c.x - a.x) *
                    (b.z - a.z)
                 -
                 (c.z - a.z) *
                    (b.x - a.x))
                /
                denominator;

            t =
                ((c.x - a.x) *
                    (d.z - c.z)
                 -
                 (c.z - a.z) *
                    (d.x - a.x))
                /
                denominator;

            if (t < -Tolerance ||
                t > 1f + Tolerance ||
                u < -Tolerance ||
                u > 1f + Tolerance)
            {
                return false;
            }

            t =
                Mathf.Clamp01(t);

            return true;
        }
    }
}