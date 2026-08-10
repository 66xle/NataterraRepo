using UnityEngine;

namespace VisualPath
{
    public static class HexPathGeometry
    {
        public static bool TryCreatePortal(VisualHex a, VisualHex b, VisualPathSettings settings, out PathPortal portal)
        {
            if (!TryFindSharedEdge(a, b, out Vector3 edgeA, out Vector3 edgeB))
            {
                portal = null;
                return false;
            }

            Vector3 midpoint = (edgeA + edgeB) * 0.5f;

            Vector3 direction = (edgeB - edgeA).normalized;

            float edgeLength = Vector3.Distance(edgeA, edgeB);

            // Apply physical edge margin first.
            float halfLength = Mathf.Max(0f, edgeLength * 0.5f - settings.EdgeMargin);

            Vector3 pointA = midpoint - direction * halfLength;

            Vector3 pointB = midpoint + direction * halfLength;

            portal = new PathPortal(a, b, pointA, pointB);

            return true;
        }

        private static bool TryFindSharedEdge(VisualHex a, VisualHex b, out Vector3 edgeA, out Vector3 edgeB)
        {
            const float cornerTolerance = 0.01f;

            for (int i = 0; i < a.Corners.Length; i++)
            {
                Vector3 a1 = a.Corners[i];

                Vector3 a2 = a.Corners[(i + 1) % a.Corners.Length];

                for (int j = 0; j < b.Corners.Length; j++)
                {
                    Vector3 b1 = b.Corners[j];

                    Vector3 b2 =  b.Corners[(j + 1) % b.Corners.Length];

                    bool firstMatches = Vector3.Distance(a1, b1) <= cornerTolerance;

                    bool secondMatches = Vector3.Distance(a2, b2) <= cornerTolerance;

                    bool reversedFirst = Vector3.Distance(a1, b2) <= cornerTolerance;

                    bool reversedSecond = Vector3.Distance(a2, b1) <= cornerTolerance;

                    if ((firstMatches && secondMatches) || (reversedFirst && reversedSecond))
                    {
                        edgeA = a1;
                        edgeB = a2;

                        return true;
                    }
                }
            }

            edgeA = default;
            edgeB = default;

            return false;
        }
    }
}