using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public static class CorridorPathValidator
    {
        private const float Tolerance = 0.001f;

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

            float x = point.x;
            float z = point.z;

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

                if (DistanceToSegmentXZ(
                        point,
                        a,
                        b) <= Tolerance)
                {
                    return true;
                }

                bool intersects =
                    ((a.z > z) != (b.z > z)) &&
                    (x <
                     (b.x - a.x) *
                     (z - a.z) /
                     (b.z - a.z) +
                     a.x);

                if (intersects)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

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

            if (Mathf.Abs(denominator) <
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
                    (d.x - c.x))
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