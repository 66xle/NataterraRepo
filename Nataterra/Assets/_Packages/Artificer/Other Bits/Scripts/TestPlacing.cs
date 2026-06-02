using UnityEngine;
using System.Collections;

namespace Artifice
{
	public class TestPlacing : Placing
	{
		ParticleSystem	particles;

		void Start()
		{
			particles = GetComponent<ParticleSystem>();
		}

		public override void PlacingElement(MeshElement me, int index, Matrix4x4 tm, float alpha)
		{
			var emitParams = new ParticleSystem.EmitParams();
			emitParams.position = tm.GetPosition();
			particles.Emit(emitParams, 1);

			//Debug.Log("Placing " + index + " at " + tm.GetPosition());
		}

		public override void PlacedElement(MeshElement me, int index, Matrix4x4 tm)
		{
		}

		public override void StartingElement(MeshElement me, int index, Matrix4x4 tm)
		{
		}
	}
}