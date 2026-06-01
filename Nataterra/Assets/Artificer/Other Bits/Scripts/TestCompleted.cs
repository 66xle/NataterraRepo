using System.Collections;
using UnityEngine;

namespace Artifice
{
	public class TestCompleted : MonoBehaviour
	{

		public void Completed(Artificer arti, GameObject obj)
		{
			Debug.Log("Completed Event " + obj);
		}
	}
}