using UnityEngine;
using System.Collections;

namespace Artifice
{
	public class Target: MonoBehaviour
	{
		Builder builder;
		public float	camX;
		public float	camY;
		public float	camZ;
		public float	spinSpeed	= 0.0f;

		public float	aperture	= 2.0f;
		public float	focalLength	= 250.0f;

		void Start()
		{
			builder = GetComponent<Builder>();
		}

		public void DoUpdate()
		{
			if ( builder )
			{
				builder.DoUpdate();
			}
		}
	}
}