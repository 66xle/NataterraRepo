using UnityEngine;

namespace Artifice
{
	public class CustomBuild : MonoBehaviour
	{
		public virtual void AddedToBuild(MeshElement me, int index)
		{
		}

		public virtual void Built(MeshElement me, int index)
		{
		}

		public virtual void Split(MeshElement me, int index)
		{
		}

		public virtual void Place(MeshElement me, float alpha, int index, out Matrix4x4 tm, out Color col)
		{
			Vector3 dir = (me.center - me.origin).normalized;
			Vector3 p = (dir * me.buildDist * (1.0f - alpha));

			tm = Matrix4x4.identity;
			col = Color.white;
		}
	}
}