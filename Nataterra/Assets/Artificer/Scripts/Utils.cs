using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice
{
#if UNITY_2023_1_OR_NEWER
#else
	public enum FindObjectsInactive
	{
		Exclude,
		Include,
	}

	public enum FindObjectsSortMode
	{
		None,
		InstanceID,
	}

#endif
	public class Utils : MonoBehaviour
	{
		static public T[] FindObjectsByType<T>(FindObjectsInactive findObjsInactive, FindObjectsSortMode sortmode) where T : Object
		{
#if UNITY_2023_1_OR_NEWER
			return Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
			if ( findObjsInactive == FindObjectsInactive.Include )
			{
				return Object.FindObjectsOfType<T>(true);
			}
			else
				return Object.FindObjectsOfType<T>(false);
#endif
		}

		// Start is called before the first frame update
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
        
		}
	}
}
