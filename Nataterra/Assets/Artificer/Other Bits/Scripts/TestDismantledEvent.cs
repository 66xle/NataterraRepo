using UnityEngine;
using Artifice;

namespace Artifice
{
	public class TestDismantledEvent : MonoBehaviour
	{
		public Artificer	artificer;

		void Start()
		{
			Artificer.DismantledEvent.AddListener(DismantledStatic);
		}

		void DismantledStatic(Artificer art)
		{
			Debug.Log("Object Dismantled");
		}
	}
}