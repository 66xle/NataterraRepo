using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class VisualRoad
    {
        public int Id { get; }

        public IReadOnlyList<Vector3> Points { get; }

        /// <summary>
        /// Hex ID corresponding to each road point.
        ///
        /// PointHexIds[i] belongs to Points[i].
        /// </summary>
        public IReadOnlyList<int> PointHexIds { get; }

        /// <summary>
        /// All hexes occupied by this road.
        /// </summary>
        public HashSet<int> HexIds { get; }

        public VisualRoad(
            int id,
            IReadOnlyList<Vector3> points,
            IReadOnlyList<int> pointHexIds)
        {
            Id = id;

            Points =
                points ??
                new List<Vector3>();

            PointHexIds =
                pointHexIds ??
                new List<int>();

            HexIds =
                new HashSet<int>();

            for (int i = 0;
                 i < PointHexIds.Count;
                 i++)
            {
                HexIds.Add(
                    PointHexIds[i]);
            }
        }
    }
}