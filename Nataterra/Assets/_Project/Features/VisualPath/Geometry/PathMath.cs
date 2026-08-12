using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public static class PathMath
    {
        public static Vector3 ProjectOnSegment(
            Vector3 point,
            Vector3 a,
            Vector3 b,
            out float t)
        {
            Vector3 direction = b - a;

            float lengthSq =
                direction.sqrMagnitude;

            if (lengthSq <= Mathf.Epsilon)
            {
                t = 0f;
                return a;
            }

            t =
                Vector3.Dot(
                    point - a,
                    direction)
                /
                lengthSq;

            t =
                Mathf.Clamp01(t);

            return a + direction * t;
        }

        public static float DistanceToSegment(
            Vector3 point,
            Vector3 a,
            Vector3 b)
        {
            Vector3 projection =
                ProjectOnSegment(
                    point,
                    a,
                    b,
                    out _);

            return Vector3.Distance(
                point,
                projection);
        }

        public static float CalculateLength(
            IReadOnlyList<Vector3> points)
        {
            if (points == null ||
                points.Count < 2)
            {
                return 0f;
            }

            float length = 0f;

            for (int i = 0;
                 i < points.Count - 1;
                 i++)
            {
                length +=
                    Vector3.Distance(
                        points[i],
                        points[i + 1]);
            }

            return length;
        }

        /// <summary>
        /// Finds the closest point on segment AB to segment CD.
        /// This is performed in the XZ plane.
        /// </summary>
        public static float SegmentDistanceXZ(
            Vector3 a,
            Vector3 b,
            Vector3 c,
            Vector3 d,
            out Vector3 pointOnAB,
            out Vector3 pointOnCD)
        {
            // First check for intersection.
            if (TrySegmentIntersectionXZ(
                    a,
                    b,
                    c,
                    d,
                    out pointOnAB))
            {
                pointOnCD = pointOnAB;

                return 0f;
            }

            float bestDistance =
                float.MaxValue;

            pointOnAB = a;
            pointOnCD = c;

            CheckCandidate(
                a,
                b,
                c,
                d,
                a,
                ref bestDistance,
                ref pointOnAB,
                ref pointOnCD);

            CheckCandidate(
                a,
                b,
                c,
                d,
                b,
                ref bestDistance,
                ref pointOnAB,
                ref pointOnCD);

            CheckCandidate(
                c,
                d,
                a,
                b,
                c,
                ref bestDistance,
                ref pointOnCD,
                ref pointOnAB);

            CheckCandidate(
                c,
                d,
                a,
                b,
                d,
                ref bestDistance,
                ref pointOnCD,
                ref pointOnAB);

            return bestDistance;
        }

        private static void CheckCandidate(
            Vector3 sourceA,
            Vector3 sourceB,
            Vector3 targetA,
            Vector3 targetB,
            Vector3 point,
            ref float bestDistance,
            ref Vector3 bestSource,
            ref Vector3 bestTarget)
        {
            Vector3 projection =
                ProjectOnSegment(
                    point,
                    targetA,
                    targetB,
                    out _);

            float distance =
                DistanceXZ(
                    point,
                    projection);

            if (distance < bestDistance)
            {
                bestDistance = distance;

                bestSource = point;

                bestTarget = projection;
            }
        }

        public static bool TrySegmentIntersectionXZ(
            Vector3 a,
            Vector3 b,
            Vector3 c,
            Vector3 d,
            out Vector3 intersection)
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
                intersection = default;

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

            float t =
                ((c.x - a.x) *
                    (d.z - c.z)
                 -
                 (c.z - a.z) *
                    (d.x - c.x))
                /
                denominator;

            if (t < 0f ||
                t > 1f ||
                u < 0f ||
                u > 1f)
            {
                intersection = default;

                return false;
            }

            intersection =
                a +
                (b - a) * t;

            return true;
        }

        public static float DistanceXZ(
            Vector3 a,
            Vector3 b)
        {
            float x =
                a.x - b.x;

            float z =
                a.z - b.z;

            return Mathf.Sqrt(
                x * x +
                z * z);
        }
    }
}