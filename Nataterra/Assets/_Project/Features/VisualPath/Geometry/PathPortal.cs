using UnityEngine;

namespace VisualPath
{
    public sealed class PathPortal
    {
        public VisualHex HexA { get; }

        public VisualHex HexB { get; }

        public Vector3 PointA { get; }

        public Vector3 PointB { get; }

        public Vector3 Center { get; }

        public Vector3 Direction { get; }

        public float Length { get; }

        public PathPortal(
            VisualHex hexA,
            VisualHex hexB,
            Vector3 pointA,
            Vector3 pointB)
        {
            HexA = hexA;
            HexB = hexB;

            PointA = pointA;
            PointB = pointB;

            Center =
                (pointA + pointB) * 0.5f;

            Vector3 direction =
                pointB - pointA;

            Length =
                direction.magnitude;

            Direction =
                Length > Mathf.Epsilon
                    ? direction / Length
                    : Vector3.zero;
        }

        public Vector3 GetPoint(float t)
        {
            return Vector3.Lerp(
                PointA,
                PointB,
                Mathf.Clamp01(t));
        }
    }
}