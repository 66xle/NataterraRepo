using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class VisualPathRequest
    {
        public readonly IReadOnlyList<VisualHex> Corridor;
        public readonly IReadOnlyList<VisualHex> AllHexes;

        public readonly Vector3 Start;
        public readonly Vector3 End;

        public readonly IReadOnlyList<VisualRoad> Roads;

        public VisualPathRequest(
            IReadOnlyList<VisualHex> corridor,
            Vector3 start,
            Vector3 end,
            IReadOnlyList<VisualRoad> roads = null,
            IReadOnlyList<VisualHex> allHexes = null)
        {
            Corridor =
                corridor;

            AllHexes =
                allHexes;

            Start =
                start;

            End =
                end;

            Roads =
                roads;
        }
    }
}