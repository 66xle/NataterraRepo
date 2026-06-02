using UnityEngine;
using System.Collections;

namespace Artifice
{
	public static class BoundsExtensions
	{
		public static Vector3 FurthestPoint(this Bounds bounds, Vector3 point)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;

			float x = Mathf.Abs(point.x - min.x) > Mathf.Abs(point.x - max.x) ? min.x : max.x;
			float y = Mathf.Abs(point.y - min.y) > Mathf.Abs(point.y - max.y) ? min.y : max.y;
			float z = Mathf.Abs(point.z - min.z) > Mathf.Abs(point.z - max.z) ? min.z : max.z;

			return new Vector3(x, y, z);
		}
	}
}