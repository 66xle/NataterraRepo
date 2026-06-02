using UnityEngine;

namespace Artifice
{
	[AddComponentMenu("Artificer/Split Params")]
	public class SplitParams : MonoBehaviour
	{
		public bool				applyToChildren	= true;
		public SplitOptions		splitOptions;
	}
}