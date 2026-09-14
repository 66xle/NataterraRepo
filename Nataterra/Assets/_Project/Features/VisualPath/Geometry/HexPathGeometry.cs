using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public static class HexPathGeometry
    {
        private const float CornerTolerance = 0.01f;

        // =============================================================
        // PORTAL
        // =============================================================

        public static bool TryCreatePortal(
            VisualHex a,
            VisualHex b,
            VisualPathSettings settings,
            out PathPortal portal)
        {
            if (!TryFindSharedEdge(
                    a,
                    b,
                    out Vector3 edgeA,
                    out Vector3 edgeB))
            {
                portal = null;
                return false;
            }

            Vector3 midpoint =
                (edgeA + edgeB) * 0.5f;

            Vector3 direction =
                (edgeB - edgeA).normalized;

            float edgeLength =
                Vector3.Distance(
                    edgeA,
                    edgeB);

            float halfLength =
                Mathf.Max(
                    0f,
                    edgeLength * 0.5f -
                    settings.EdgeMargin);

            Vector3 pointA =
                midpoint -
                direction *
                halfLength;

            Vector3 pointB =
                midpoint +
                direction *
                halfLength;

            portal =
                new PathPortal(
                    a,
                    b,
                    pointA,
                    pointB);

            return true;
        }

        // =============================================================
        // SHARED EDGE
        // =============================================================

        public static bool TryGetSharedEdge(
            VisualHex a,
            VisualHex b,
            out Vector3 edgeA,
            out Vector3 edgeB)
        {
            return TryFindSharedEdge(
                a,
                b,
                out edgeA,
                out edgeB);
        }

        private static bool TryFindSharedEdge(
            VisualHex a,
            VisualHex b,
            out Vector3 edgeA,
            out Vector3 edgeB)
        {
            if (a.Corners == null ||
                b.Corners == null ||
                a.Corners.Length < 2 ||
                b.Corners.Length < 2)
            {
                edgeA = default;
                edgeB = default;

                return false;
            }

            for (int i = 0;
                 i < a.Corners.Length;
                 i++)
            {
                Vector3 a1 =
                    a.Corners[i];

                Vector3 a2 =
                    a.Corners[
                        (i + 1) %
                        a.Corners.Length];

                for (int j = 0;
                     j < b.Corners.Length;
                     j++)
                {
                    Vector3 b1 =
                        b.Corners[j];

                    Vector3 b2 =
                        b.Corners[
                            (j + 1) %
                            b.Corners.Length];

                    bool sameDirection =
                        Vector3.Distance(
                            a1,
                            b1) <=
                        CornerTolerance &&
                        Vector3.Distance(
                            a2,
                            b2) <=
                        CornerTolerance;

                    bool oppositeDirection =
                        Vector3.Distance(
                            a1,
                            b2) <=
                        CornerTolerance &&
                        Vector3.Distance(
                            a2,
                            b1) <=
                        CornerTolerance;

                    if (sameDirection ||
                        oppositeDirection)
                    {
                        edgeA =
                            a1;

                        edgeB =
                            a2;

                        return true;
                    }
                }
            }

            edgeA = default;
            edgeB = default;

            return false;
        }

        // =============================================================
        // EDGES
        // =============================================================

        public static List<Vector3[]> GetEdges(
            VisualHex hex)
        {
            var edges =
                new List<Vector3[]>();

            if (hex.Corners == null ||
                hex.Corners.Length < 2)
            {
                return edges;
            }

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

                edges.Add(
                    new[]
                    {
                        a,
                        b
                    });
            }

            return edges;
        }

        // =============================================================
        // SAME EDGE
        // =============================================================

        public static bool AreSameEdge(
            Vector3 a1,
            Vector3 a2,
            Vector3 b1,
            Vector3 b2)
        {
            bool sameDirection =
                Vector3.Distance(
                    a1,
                    b1) <=
                CornerTolerance &&
                Vector3.Distance(
                    a2,
                    b2) <=
                CornerTolerance;

            bool oppositeDirection =
                Vector3.Distance(
                    a1,
                    b2) <=
                CornerTolerance &&
                Vector3.Distance(
                    a2,
                    b1) <=
                CornerTolerance;

            return
                sameDirection ||
                oppositeDirection;
        }

        // =============================================================
        // BOUNDARY EDGE
        // =============================================================

        /// <summary>
        /// Returns true when this edge does not have a neighbouring
        /// hex in the supplied map.
        /// </summary>
        public static bool IsBoundaryEdge(
            VisualHex hex,
            Vector3 edgeA,
            Vector3 edgeB,
            IReadOnlyList<VisualHex> allHexes)
        {
            if (allHexes == null)
                return false;

            for (int i = 0;
                 i < allHexes.Count;
                 i++)
            {
                VisualHex other =
                    allHexes[i];

                if (other.Id == hex.Id)
                    continue;

                if (TryGetSharedEdge(
                        hex,
                        other,
                        out Vector3 sharedA,
                        out Vector3 sharedB))
                {
                    if (AreSameEdge(
                            edgeA,
                            edgeB,
                            sharedA,
                            sharedB))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // =============================================================
        // EDGE DIRECTION
        // =============================================================

        public static Vector3 GetEdgeDirection(
            Vector3 edgeA,
            Vector3 edgeB)
        {
            Vector3 direction =
                edgeB - edgeA;

            direction.y = 0f;

            if (direction.sqrMagnitude <=
                0.000001f)
            {
                return Vector3.zero;
            }

            return direction.normalized;
        }

        // =============================================================
        // PERPENDICULAR
        // =============================================================

        public static Vector3 GetPerpendicular(
            Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude <=
                0.000001f)
            {
                return Vector3.zero;
            }

            direction.Normalize();

            return new Vector3(
                -direction.z,
                0f,
                direction.x);
        }
    }
}