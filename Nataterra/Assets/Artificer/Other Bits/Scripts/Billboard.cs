using UnityEngine;

namespace Artifice
{
	public class Billboard : MonoBehaviour
	{
		private void LateUpdate()
		{
			var lookPos = Camera.main.transform.position - transform.position;
			lookPos.y = 0;
			Quaternion rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
			transform.rotation = Quaternion.LookRotation(lookPos) * rot;
		}
	}
}