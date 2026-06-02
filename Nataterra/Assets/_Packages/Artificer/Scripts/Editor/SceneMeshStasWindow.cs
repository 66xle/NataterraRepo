using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Artifice
{
	public class SceneMeshStatsWindow : EditorWindow
	{
		private int sceneVertices;
		private int sceneTriangles;
		private int selectionVertices;
		private int selectionTriangles;
		bool		hasTangents;
		bool		hasColors;
		bool		selHasTangents;
		bool		selHasColors;
		bool		selHasUV;
		bool		selHasUV2;
		int			selSubmeshes;
		int			selMaterials;

		private bool includeChildren = true;

		[MenuItem("Tools/Scene Mesh Stats")]
		public static void ShowWindow()
		{
			GetWindow<SceneMeshStatsWindow>("Mesh Stats");
		}

		private void OnEnable()
		{
			Selection.selectionChanged += Recalculate;
			EditorSceneManager.sceneOpened += (_, __) => Recalculate();
			EditorSceneManager.sceneClosed += (_) => Recalculate();
			Recalculate();
		}

		private void OnDisable()
		{
			Selection.selectionChanged -= Recalculate;
		}

		private void OnGUI()
		{
			GUILayout.Label("Scene Mesh Stats", EditorStyles.boldLabel);

			GUILayout.Space(5);
			GUILayout.Label("Entire Scene", EditorStyles.boldLabel);
			GUILayout.Label($"Vertices: {sceneVertices:N0}");
			GUILayout.Label($"Triangles: {sceneTriangles:N0}");
			GUILayout.Label($"Has Tangents: {hasTangents}");
			GUILayout.Label($"Has Colors: {hasColors}");

			GUILayout.Space(10);
			GUILayout.Label("Selection", EditorStyles.boldLabel);

			EditorGUI.BeginChangeCheck();
			includeChildren = EditorGUILayout.Toggle("Include Children", includeChildren);
			if ( EditorGUI.EndChangeCheck() )
			{
				Recalculate();
			}

			GUILayout.Label($"Vertices: {selectionVertices:N0}");
			GUILayout.Label($"Triangles: {selectionTriangles:N0}");
			GUILayout.Label($"Has Tangents: {selHasTangents}");
			GUILayout.Label($"Has Colors: {selHasColors}");
			GUILayout.Label($"Has UV: {selHasUV}");
			GUILayout.Label($"Has UV2: {selHasUV2}");
			GUILayout.Label($"Submeshes: {selSubmeshes:N0}");
			GUILayout.Label($"Materials: {selMaterials:N0}");

			GUILayout.Space(10);
			if (GUILayout.Button("Recalculate"))
			{
				Recalculate();
			}
		}

		private void Recalculate()
		{
			sceneVertices		= 0;
			sceneTriangles		= 0;
			selectionVertices	= 0;
			selectionTriangles	= 0;
			hasTangents			= false;
			hasColors			= false;
			selHasTangents		= false;
			selHasColors		= false;
			selSubmeshes		= 0;
			selMaterials		= 0;
			selHasUV			= false;
			selHasUV2			= false;


			// ----- Entire scene -----
			var meshFilters = Utils.FindObjectsByType<MeshFilter>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach ( var mf in meshFilters )
			{
				if ( mf.sharedMesh == null ) continue;
				sceneVertices += mf.sharedMesh.vertexCount;
				sceneTriangles += mf.sharedMesh.triangles.Length / 3;

				if ( mf.sharedMesh.tangents != null && mf.sharedMesh.tangents.Length > 0 )
					hasTangents = true;

				if ( mf.sharedMesh.colors != null && mf.sharedMesh.colors.Length > 0 )
					hasColors = true;
			}

			// ----- Selection -----
			foreach ( var go in Selection.gameObjects )
			{
				if ( includeChildren )
				{
					var filters = go.GetComponentsInChildren<MeshFilter>();
					foreach (var mf in filters)
					{
						if (mf.sharedMesh == null) continue;
						selectionVertices += mf.sharedMesh.vertexCount;
						selectionTriangles += mf.sharedMesh.triangles.Length / 3;
						selSubmeshes += mf.sharedMesh.subMeshCount;
						Renderer mr = mf.GetComponent<Renderer>();
						selMaterials += mr.sharedMaterials.Length;

						if ( mf.sharedMesh.tangents != null && mf.sharedMesh.tangents.Length > 0 )
							selHasTangents = true;

						if ( mf.sharedMesh.colors != null && mf.sharedMesh.colors.Length > 0 )
							selHasColors = true;

						if ( mf.sharedMesh.uv != null && mf.sharedMesh.uv.Length > 0 )
							selHasUV = true;

						if ( mf.sharedMesh.uv2 != null && mf.sharedMesh.uv2.Length > 0 )
							selHasUV2 = true;
					}
				}
				else
				{
					var mf = go.GetComponent<MeshFilter>();
					if ( mf == null || mf.sharedMesh == null ) continue;
					selectionVertices += mf.sharedMesh.vertexCount;
					selectionTriangles += mf.sharedMesh.triangles.Length / 3;

					if ( mf.sharedMesh.tangents != null && mf.sharedMesh.tangents.Length > 0 )
						selHasTangents = true;

					if ( mf.sharedMesh.colors != null && mf.sharedMesh.colors.Length > 0 )
						selHasColors = true;
				}
			}

			Repaint();
		}
	}
}