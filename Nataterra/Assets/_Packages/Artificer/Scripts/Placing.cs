using UnityEngine;

namespace Artifice
{
	public class Placing : MonoBehaviour
	{
		public virtual void PlacingElement(MeshElement me, int index, Matrix4x4 tm, float alpha)
		{
		}

		public virtual void PlacedElement(MeshElement me, int index, Matrix4x4 tm)
		{
		}

		public virtual void StartingElement(MeshElement me, int index, Matrix4x4 tm)
		{
		}
	}
}