using UnityEngine;

namespace Artifice
{
	public class CustomDismantle : MonoBehaviour
	{
		public virtual void AddedToDismantle(MeshElement me, int index)
		{
		}

		public virtual void Dismantled(MeshElement me, int index)
		{
		}

		public virtual void Split(MeshElement me, int index)
		{
		}

		public virtual void Remove(MeshElement me, float alpha, int index, out Matrix4x4 tm, out Color col)
		{
			tm = Matrix4x4.identity;
			col = Color.white;
		}
	}
}