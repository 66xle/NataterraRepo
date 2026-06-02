using UnityEngine;
using Artifice;

namespace Artifice
{
	public class TestPlacedEvents : MonoBehaviour
	{
		public Artificer	artificer;

		void Start()
		{
			artificer.placedEvent.AddListener(Built);
			artificer.builtEvent.AddListener(AllBuilt);

			Artificer.PlacedEvent.AddListener(BuiltStatic);
		}

		void AllBuilt(Artificer art)
		{
			Debug.Log("Fully Built");
		}

		void Built(Artificer art, int index)
		{
			Debug.Log("We built piece " + index);
		}

		void BuiltStatic(Artificer art, int index)
		{
			Debug.Log("We built piece static " + index);
		}

		public void PlaceEventTest(Artificer art, int index)
		{
			Debug.Log("Placed " + index);
		}
	}
}