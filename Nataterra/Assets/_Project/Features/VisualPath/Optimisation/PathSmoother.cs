using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public static class PathSmoother
    {
        public static List<Vector3> SmoothTurns(IReadOnlyList<Vector3> points, float ratio)
        {
            if (points.Count <= 2)
                return new List<Vector3>(points);

            ratio = Mathf.Clamp01(ratio);

            var result = new List<Vector3>();

            result.Add(points[0]);

            for (int i = 1; i < points.Count - 1; i++)
            {
                Vector3 previous = points[i - 1];

                Vector3 current = points[i];

                Vector3 next = points[i + 1];

                Vector3 beforeTurn = Vector3.Lerp(current, previous, ratio);

                Vector3 afterTurn = Vector3.Lerp(current, next, ratio);

                result.Add(beforeTurn);
                result.Add(current);
                result.Add(afterTurn);
            }

            result.Add(points[points.Count - 1]);

            return result;
        }
    }
}