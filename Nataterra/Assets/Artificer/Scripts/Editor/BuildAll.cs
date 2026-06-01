using UnityEngine;
using UnityEditor;

namespace Artifice
{
	public class BuildAll
	{
		[MenuItem("Tools/Artificer/Build All In Scene")]
		static void Build()
		{
			//Artificer[]	all = Object.FindObjectsByType<Artificer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			Artificer[]	all = Utils.FindObjectsByType<Artificer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

			for ( int i = 0; i < all.Length; i++ )
			{
				if ( all[i].isActiveAndEnabled )
				{
					EditorUtility.DisplayProgressBar("Building All Atrificer Objects", "Building " + all[i].name, (float)i / (float)all.Length);

					all[i].BuildData();
					if ( all[i].buildData )
						EditorUtility.SetDirty(all[i].buildData);
				}
			}

			EditorUtility.ClearProgressBar();

			AssetDatabase.SaveAssets();
		}

		[MenuItem("Tools/Artificer/Build Dirty In Scene")]
		static void BuildDirty()
		{
			//Artificer[]	all = Object.FindObjectsByType<Artificer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			Artificer[]	all = Utils.FindObjectsByType<Artificer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

			for ( int i = 0; i < all.Length; i++ )
			{
				EditorUtility.DisplayProgressBar("Building Dirty Atrificer Objects", "Building " + all[i].name, (float)i / (float)all.Length);
				if ( all[i].isActiveAndEnabled && all[i].rebuildNeeded )
				{
					all[i].BuildData();

					if ( all[i].buildData )
						EditorUtility.SetDirty(all[i].buildData);
				}
			}

			EditorUtility.ClearProgressBar();
			AssetDatabase.SaveAssets();
		}
	}
}