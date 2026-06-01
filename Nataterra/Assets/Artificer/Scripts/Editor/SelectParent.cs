using UnityEditor;
using UnityEngine;

namespace Artifice
{
	public static class SelectParent
	{
		[MenuItem("Edit/Select Parent %Q")] // Ctrl+A
		static void Select()
		{
			if ( Selection.activeTransform )
			{
				Artificer art = Selection.activeTransform.GetComponentInParent<Artificer>();

				if ( art )
				{
					Selection.activeGameObject = art.gameObject;
				}
				else
				{
					if ( Selection.activeTransform?.parent != null )
						Selection.activeGameObject = Selection.activeTransform.parent.gameObject;
				}
			}
		}
	}
}