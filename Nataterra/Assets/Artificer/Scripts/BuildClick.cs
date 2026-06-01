using UnityEngine;
using UnityEngine.EventSystems;

namespace Artifice
{
	public class BuildClick : MonoBehaviour, IPointerClickHandler
	{
		[HideInInspector]
		public Artificer	artificer;	// Will be set by Artificer when the prefab is added to the scene

		public void OnPointerClick(PointerEventData eventData)
		{
			artificer.buildBit = true;
		}
	}
}