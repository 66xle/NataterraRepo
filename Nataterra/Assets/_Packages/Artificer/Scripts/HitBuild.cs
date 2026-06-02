using UnityEngine;

namespace Artifice
{
	public class HitBuild : MonoBehaviour
	{
		public Artificer	build;

		private void OnCollisionEnter(Collision collision)
		{
			build.buildMode = BuildMode.Build;

			gameObject.SetActive(false);
		}
	}
}
