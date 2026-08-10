using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class VisualPathResult
    {
        public IReadOnlyList<Vector3> RawPoints { get; }
        public IReadOnlyList<Vector3> OptimizedPoints { get; }
        public IReadOnlyList<Vector3> FinalPoints { get; }
        public float Length { get; }
        public int TurnCount { get; }
        public bool UsedRoad { get; }
        public bool IsValid { get; }
        public string Error { get; }

        private VisualPathResult(List<Vector3> rawPoints, List<Vector3> optimizedPoints, List<Vector3> finalPoints, bool usedRoad)
        {
            RawPoints = rawPoints;
            OptimizedPoints = optimizedPoints;
            FinalPoints = finalPoints;

            Length = PathMath.CalculateLength(finalPoints);
            TurnCount = CountTurns(optimizedPoints);

            UsedRoad = usedRoad;
            IsValid = true;
        }

        private VisualPathResult(string error)
        {
            Error = error;
            IsValid = false;

            RawPoints = new List<Vector3>();
            OptimizedPoints = new List<Vector3>();
            FinalPoints = new List<Vector3>();
        }

        public static VisualPathResult Create(List<Vector3> rawPoints, List<Vector3> optimizedPoints, List<Vector3> finalPoints, bool usedRoad)
        {
            return new VisualPathResult(rawPoints, optimizedPoints, finalPoints, usedRoad);
        }

        public static VisualPathResult Failed(string error)
        {
            return new VisualPathResult(error);
        }

        private static int CountTurns(IReadOnlyList<Vector3> points)
        {
            int turns = 0;

            for (int i = 1; i < points.Count - 1; i++)
            {
                Vector3 a = points[i] - points[i - 1];
                Vector3 b = points[i + 1] - points[i];

                if (a.sqrMagnitude <= Mathf.Epsilon || b.sqrMagnitude <= Mathf.Epsilon)
                {
                    continue;
                }

                float angle = Vector3.Angle(a, b);

                if (angle > 6f)
                    turns++;
            }

            return turns;
        }
    }
}
