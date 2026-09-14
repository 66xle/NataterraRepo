using UnityEngine;

namespace VisualPath
{
    public readonly struct NavMeshBlockerRequest
    {
        public readonly VisualHex CorridorHex;

        public readonly Vector3 EdgeA;
        public readonly Vector3 EdgeB;

        public NavMeshBlockerRequest(
            VisualHex corridorHex,
            Vector3 edgeA,
            Vector3 edgeB)
        {
            CorridorHex =
                corridorHex;

            EdgeA =
                edgeA;

            EdgeB =
                edgeB;
        }

        public Vector3 Midpoint
        {
            get
            {
                return (EdgeA + EdgeB) * 0.5f;
            }
        }

        public float EdgeLength
        {
            get
            {
                return PathMath.DistanceXZ(
                    EdgeA,
                    EdgeB);
            }
        }
    }
}