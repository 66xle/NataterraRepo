using UnityEngine;

namespace VisualPath
{
    public readonly struct VisualHex
    {
        public readonly int Id;
        public readonly Vector3 Center;
        public readonly Vector3[] Corners;

        public VisualHex(int id, Vector3 center, Vector3[] corners)
        {
            Id = id;
            Center = center;
            Corners = corners;
        }
    }
}