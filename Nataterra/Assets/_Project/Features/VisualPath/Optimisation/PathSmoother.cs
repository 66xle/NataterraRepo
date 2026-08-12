using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public static class PathSmoother
    {
        /// <summary>
        /// Smooths a path using Catmull-Rom interpolation.
        ///
        /// The supplied points remain part of the resulting path.
        /// Additional points are generated between them so a
        /// LineRenderer can display the path as a smooth curve.
        /// </summary>
        public static List<Vector3> SmoothTurns(
            IReadOnlyList<Vector3> points,
            float ratio,
            int samplesPerSegment = 8)
        {
            if (points == null ||
                points.Count == 0)
            {
                return new List<Vector3>();
            }

            if (points.Count <= 2)
            {
                return new List<Vector3>(points);
            }

            ratio = Mathf.Clamp01(ratio);

            samplesPerSegment =
                Mathf.Max(1, samplesPerSegment);

            /*
             * No smoothing requested.
             */
            if (ratio <= 0.001f)
            {
                return new List<Vector3>(points);
            }

            var result =
                new List<Vector3>(
                    (points.Count - 1) *
                    samplesPerSegment +
                    1);

            /*
             * Always preserve the exact first point.
             */
            result.Add(points[0]);

            for (int i = 0;
                 i < points.Count - 1;
                 i++)
            {
                Vector3 p0 =
                    i > 0
                        ? points[i - 1]
                        : points[i];

                Vector3 p1 =
                    points[i];

                Vector3 p2 =
                    points[i + 1];

                Vector3 p3 =
                    i + 2 < points.Count
                        ? points[i + 2]
                        : points[i + 1];

                for (int sample = 1;
                     sample <= samplesPerSegment;
                     sample++)
                {
                    float t =
                        sample /
                        (float)samplesPerSegment;

                    Vector3 point =
                        CatmullRom(
                            p0,
                            p1,
                            p2,
                            p3,
                            t,
                            ratio);

                    AddUnique(
                        result,
                        point);
                }
            }

            /*
             * Guarantee exact endpoint.
             */
            if (result.Count > 0)
            {
                result[result.Count - 1] =
                    points[points.Count - 1];
            }

            return result;
        }

        private static Vector3 CatmullRom(
            Vector3 p0,
            Vector3 p1,
            Vector3 p2,
            Vector3 p3,
            float t,
            float ratio)
        {
            float t2 =
                t * t;

            float t3 =
                t2 * t;

            /*
             * Standard Catmull-Rom tangents.
             *
             * Ratio controls how strongly the neighbouring
             * points influence the curve.
             */
            Vector3 tangent1 =
                (p2 - p0) *
                (0.5f * ratio);

            Vector3 tangent2 =
                (p3 - p1) *
                (0.5f * ratio);

            float h00 =
                2f * t3 -
                3f * t2 +
                1f;

            float h10 =
                t3 -
                2f * t2 +
                t;

            float h01 =
                -2f * t3 +
                3f * t2;

            float h11 =
                t3 -
                t2;

            return
                h00 * p1 +
                h10 * tangent1 +
                h01 * p2 +
                h11 * tangent2;
        }

        private static void AddUnique(
            List<Vector3> result,
            Vector3 point)
        {
            if (result.Count == 0)
            {
                result.Add(point);
                return;
            }

            if (PathMath.DistanceXZ(
                    result[result.Count - 1],
                    point) > 0.001f)
            {
                result.Add(point);
            }
        }
    }
}