using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Artifice
{
	public class MeshCombine : EditorWindow
	{
		[MenuItem("Tools/Artificer/Mesh/Mesh Combiner")]
		private static void ShowWindow()
		{
			GetWindow<MeshCombine>("Combine Meshes");
		}

		private GameObject targetObject;
		bool		recalcNormals = false;
		Texture		logoImage;

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
			recalcNormals = EditorGUILayout.Toggle("Recalc Normals", recalcNormals);

			if ( GUILayout.Button("Combine Meshes") )
			{
				if ( targetObject == null )
				{
					Debug.LogWarning("Please assign a target GameObject.");
					return;
				}

				Combine(targetObject, recalcNormals);
			}
		}


		void RebuildAll()
		{
			//Artificer[] artis = FindObjectsByType<Artificer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			Artificer[] artis = Utils.FindObjectsByType<Artificer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

			for ( int i = 0; i < artis.Length; i++ )
			{
				artis[i].BuildData();
			}
		}

		GameObject Combine(GameObject gameObject, bool recalc)
		{
			GameObject newobj = new GameObject();
			newobj.name = gameObject.name + " Combined";
			newobj.transform.position = gameObject.transform.position;
			newobj.transform.rotation = gameObject.transform.rotation;

			Vector3 basePosition = gameObject.transform.position;
			Quaternion baseRotation = gameObject.transform.rotation;
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;

			ArrayList materials = new ArrayList();
			ArrayList combineInstanceArrays = new ArrayList();
			MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

			foreach ( MeshFilter meshFilter in meshFilters )
			{
				MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();

				if ( !meshRenderer || !meshFilter.sharedMesh || meshRenderer.sharedMaterials.Length != meshFilter.sharedMesh.subMeshCount )
					continue;

				for ( int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++ )
				{
					int materialArrayIndex = Contains(materials, meshRenderer.sharedMaterials[s].name);
					if ( materialArrayIndex == -1 )
					{
						materials.Add(meshRenderer.sharedMaterials[s]);
						materialArrayIndex = materials.Count - 1;
					}
					combineInstanceArrays.Add(new ArrayList());

					CombineInstance combineInstance = new CombineInstance();
					combineInstance.transform = meshRenderer.transform.localToWorldMatrix;
					combineInstance.subMeshIndex = s;
					combineInstance.mesh = meshFilter.sharedMesh;
					(combineInstanceArrays[materialArrayIndex] as ArrayList).Add(combineInstance);
				}
			}

			MeshFilter meshFilterCombine = newobj.GetComponent<MeshFilter>();
			if ( meshFilterCombine == null )
				meshFilterCombine = newobj.AddComponent<MeshFilter>();

			MeshRenderer meshRendererCombine = newobj.GetComponent<MeshRenderer>();
			if ( meshRendererCombine == null )
				meshRendererCombine = newobj.AddComponent<MeshRenderer>();

			Mesh[] meshes = new Mesh[materials.Count];
			CombineInstance[] combineInstances = new CombineInstance[materials.Count];

			int vcount = 0;

			for ( int m = 0; m < materials.Count; m++ )
			{
				CombineInstance[] combineInstanceArray = (combineInstanceArrays[m] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];

				for ( int j = 0; j < combineInstanceArray.Length; j++ )
					vcount += combineInstanceArray[j].mesh.vertexCount;
			}

			for ( int m = 0; m < materials.Count; m++ )
			{
				CombineInstance[] combineInstanceArray = (combineInstanceArrays[m] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];
				meshes[m] = new Mesh();

				if ( vcount > 65535 )
					meshes[m].indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
				else
					meshes[m].indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;

				meshes[m].CombineMeshes(combineInstanceArray, true, true);

				combineInstances[m] = new CombineInstance();
				combineInstances[m].mesh = meshes[m];
				combineInstances[m].subMeshIndex = 0;
			}

			meshFilterCombine.sharedMesh = new Mesh();
			meshFilterCombine.sharedMesh.CombineMeshes(combineInstances, false, false);

			if ( recalc )
				meshFilterCombine.sharedMesh.RecalculateNormals();

			Material[] materialsArray = materials.ToArray(typeof(Material)) as Material[];
			meshRendererCombine.materials = materialsArray;

			gameObject.transform.position = basePosition;
			gameObject.transform.rotation = baseRotation;

			SaveMeshAsAsset(meshFilterCombine.sharedMesh, gameObject.name);
			return newobj;
		}

		static int Contains(ArrayList searchList, string searchName)
		{
			for ( int i = 0; i < searchList.Count; i++ )
			{
				if ( ((Material)searchList[i]).name == searchName )
					return i;
			}
			return -1;
		}

		static void SaveMeshAsAsset(Mesh mesh, string defaultName = "CombinedMesh")
		{
			if ( mesh == null )
			{
				Debug.LogError("Cannot save a null mesh.");
				return;
			}

			string path = EditorUtility.SaveFilePanelInProject("Save Combined Mesh", defaultName, "asset", "Choose where to save the combined mesh asset");

			if ( string.IsNullOrEmpty(path) )
				return; // User canceled

			// Make sure we don't overwrite an existing asset accidentally
			Mesh meshCopy = Object.Instantiate(mesh);
			meshCopy.name = System.IO.Path.GetFileNameWithoutExtension(path);

			AssetDatabase.CreateAsset(meshCopy, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			EditorGUIUtility.PingObject(meshCopy);

			Debug.Log($"Mesh saved to {path}");
		}
	}
}