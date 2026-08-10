using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class VisualPathRequest
    {
        public IReadOnlyList<VisualHex> Corridor { get; }
        public Vector3 Start { get; }
        public Vector3 End { get; }
        public IReadOnlyList<VisualRoad> Roads { get; }

        public VisualPathRequest(IReadOnlyList<VisualHex> corridor, Vector3 start, Vector3 end, IReadOnlyList<VisualRoad> roads = null)
        {
            Corridor = corridor;
            Start = start;
            End = end;
            Roads = roads;
        }
    }
}