using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class VisualRoad
    {
        public int Id { get; }

        public int Priority { get; }

        public IReadOnlyList<Vector3> Points { get; }

        public HashSet<int> HexIds { get; }

        public VisualRoad(int id, int priority, IReadOnlyList<Vector3> points, HashSet<int> hexIds)
        {
            Id = id;
            Priority = priority;
            Points = points;
            HexIds = hexIds;
        }
    }
}