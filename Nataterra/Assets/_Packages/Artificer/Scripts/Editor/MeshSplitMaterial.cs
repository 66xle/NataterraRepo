using UnityEngine;
using UnityEditor;

#if false
namespace Artifice
{
	[CustomEditor(typeof(MeshFilter))]
	public class SplitMeshByMaterial : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			MeshFilter mf = (MeshFilter)target;
			MeshRenderer mr = mf.GetComponent<MeshRenderer>();

			if (mf.sharedMesh == null || mr == null)
			{
				EditorGUILayout.HelpBox("MeshFilter needs a MeshRenderer with materials.", MessageType.Warning);
				return;
			}

			bool canSplit = mf.sharedMesh.subMeshCount == mr.sharedMaterials.Length;

			using (new EditorGUI.DisabledScope(!canSplit))
			{
				if (GUILayout.Button("Split Mesh By Material"))
				{
					Split(mf, mr);
				}
			}

			if (!canSplit)
			{
				EditorGUILayout.HelpBox("Submesh count must match material count.", MessageType.Info);
			}
		}

		private void Split(MeshFilter mf, MeshRenderer mr)
		{
			Mesh sourceMesh = mf.sharedMesh;
			Transform parent = mf.transform;

			Undo.RegisterFullObjectHierarchyUndo(parent.gameObject, "Split Mesh By Material");

			for (int i = 0; i < sourceMesh.subMeshCount; i++)
			{
				Mesh newMesh = new Mesh();
				newMesh.name = sourceMesh.name + "_Mat_" + i;

				newMesh.vertices = sourceMesh.vertices;
				newMesh.normals = sourceMesh.normals;
				newMesh.tangents = sourceMesh.tangents;
				newMesh.uv = sourceMesh.uv;
				newMesh.uv2 = sourceMesh.uv2;
				newMesh.colors = sourceMesh.colors;

				newMesh.triangles = sourceMesh.GetTriangles(i);
				newMesh.RecalculateBounds();

				GameObject go = new GameObject(sourceMesh.name + "_Part_" + i);
				Undo.RegisterCreatedObjectUndo(go, "Create split mesh");

				go.transform.SetParent(parent, false);

				MeshFilter newMF = go.AddComponent<MeshFilter>();
				MeshRenderer newMR = go.AddComponent<MeshRenderer>();

				newMF.sharedMesh = newMesh;
				newMR.sharedMaterial = mr.sharedMaterials[i];
			}

			// Disable original renderer
			mr.enabled = false;
		}
	}
}
#endif