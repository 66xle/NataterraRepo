using UnityEngine;

namespace Artifice
{
	public class TestCustomBuild : CustomBuild
	{
		public float	distance = 10.0f;

		public override void Place(MeshElement me, float alpha, int index, out Matrix4x4 tm, out Color col)
		{
			Vector3 dir = (me.center - me.origin).normalized;
			Vector3 p = (dir * distance * (1.0f - alpha));

			tm = Matrix4x4.TRS(p, Quaternion.identity, Vector3.one);
			col = Color.white;
		}
	}
}