using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Artifice
{
	public class MakeMeshesReadable : EditorWindow
	{
		[MenuItem("Tools/Artificer/Mesh/Make Meshes Readable")]
		private static void ShowWindow()
		{
			GetWindow<MakeMeshesReadable>("Make Meshes Readable");
		}

		private GameObject targetObject;
		Texture logoImage;

		private void OnGUI()
		{
			if ( logoImage == null )
				logoImage = (Texture)Resources.Load<Texture>("Artificer");

			if ( logoImage )
			{
				float h1 = (float)logoImage.height / ((float)logoImage.width / ((float)Screen.width - 0));

				Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(h1));
				rect.xMin = 0;
				rect.width = EditorGUIUtility.currentViewWidth;

				GUI.DrawTexture(rect, logoImage, ScaleMode.ScaleAndCrop);
			}

			targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

			if ( GUILayout.Button("Make Readable") )
			{
				if ( targetObject == null )
				{
					Debug.LogWarning("Please assign a target GameObject.");
					return;
				}

				ProcessMeshes(targetObject, true);
			}

			if ( GUILayout.Button("Make Un-Readable") )
			{
				if ( targetObject == null )
				{
					Debug.LogWarning("Please assign a target GameObject.");
					return;
				}

				ProcessMeshes(targetObject, false);
			}
		}

		public static void ProcessMeshes(GameObject root, bool readable)
		{
			HashSet<string> processedAssets = new HashSet<string>();
			int updatedCount = 0;

			SetReadable(root, processedAssets, ref updatedCount, root.GetComponentsInChildren<MeshFilter>(true), readable);
			SetReadable(root, processedAssets, ref updatedCount, root.GetComponentsInChildren<SkinnedMeshRenderer>(true), readable);

			if ( updatedCount > 0 )
			{
				AssetDatabase.Refresh();
				Debug.Log($"✅ Completed! {updatedCount} mesh assets updated & reimported.");
			}
			else
			{
				Debug.Log("✅ All meshes are already readable. No reimport required.");
			}
		}

		private static void SetReadable<T>(GameObject root, HashSet<string> processedAssets, ref int updatedCount, T[] components, bool readable) where T : Component
		{
			foreach ( var component in components )
			{
				Mesh mesh = null;

				if ( component is MeshFilter mf )
					mesh = mf.sharedMesh;
				else if ( component is SkinnedMeshRenderer smr )
					mesh = smr.sharedMesh;

				if ( mesh == null ) continue;

				string meshPath = AssetDatabase.GetAssetPath(mesh);
				if ( string.IsNullOrEmpty(meshPath) ) continue; // Skip scene-only meshes

				if ( processedAssets.Contains(meshPath) ) continue;
				processedAssets.Add(meshPath);

				var importer = AssetImporter.GetAtPath(meshPath) as ModelImporter;


				if ( importer != null )
				{
					if ( readable )
					{
						if ( !importer.isReadable )
						{
							importer.isReadable = true;
							importer.SaveAndReimport();
							updatedCount++;
							Debug.Log($"🛠️ Updated mesh: {mesh.name} ({meshPath}) set to readable.");
						}
					}
					else
					{
						if ( importer.isReadable )
						{
							importer.isReadable = false;
							importer.SaveAndReimport();
							updatedCount++;
							Debug.Log($"🛠️ Updated mesh: {mesh.name} ({meshPath}) set to non readable.");
						}
					}
				}
			}
		}
	}
}