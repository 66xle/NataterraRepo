using UnityEngine;
using System.Collections;

namespace Artifice
{
	public class TankPlacing : Placing
	{
		ParticleSystem particles;

		void Start()
		{
			particles = GetComponent<ParticleSystem>();
		}

		public override void PlacingElement(MeshElement me, int index, Matrix4x4 tm, float alpha)
		{
		}

		public override void PlacedElement(MeshElement me, int index, Matrix4x4 tm)
		{
			if ( particles )
			{
				var emitParams = new ParticleSystem.EmitParams();
				emitParams.position = tm.GetPosition();
				particles.Emit(emitParams, 1);
			}
		}

		public override void StartingElement(MeshElement me, int index, Matrix4x4 tm)
		{
		}
	}
}