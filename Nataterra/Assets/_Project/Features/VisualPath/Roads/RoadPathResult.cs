using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class RoadPathResult
    {
        public IReadOnlyList<Vector3> Points { get; }

        /// <summary>
        /// True when the corresponding point came directly from
        /// the supplied road geometry.
        ///
        /// False for normal path points and transition points.
        /// </summary>
        public IReadOnlyList<bool> IsRoadPoint { get; }

        public RoadPathResult(
            IReadOnlyList<Vector3> points,
            IReadOnlyList<bool> isRoadPoint)
        {
            Points = points;
            IsRoadPoint = isRoadPoint;
        }
    }
}