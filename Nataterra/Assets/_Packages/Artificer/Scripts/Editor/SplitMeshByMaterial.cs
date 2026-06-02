using UnityEngine;
using UnityEditor;

namespace Artifice
{
	[CustomEditor(typeof(MeshFilter))]
	public class SplitMeshByMaterial : Editor
	{
		[MenuItem("Tools/Artificer/Mesh/Split Mesh By Material")]
		private static void SplitSelected()
		{
			GameObject go = Selection.activeGameObject;

			if ( !go )
			{
				EditorUtility.DisplayDialog("No Selection", "Select a GameObject with a MeshFilter.", "OK");
				return;
			}

			MeshFilter mf = go.GetComponent<MeshFilter>();
			MeshRenderer mr = go.GetComponent<MeshRenderer>();

			if ( !mf || !mr || !mf.sharedMesh )
			{
				EditorUtility.DisplayDialog("Invalid Selection", "Selected object must have a MeshFilter and MeshRenderer.", "OK");
				return;
			}

			Mesh mesh = mf.sharedMesh;

			if ( mesh.subMeshCount != mr.sharedMaterials.Length )
			{
				EditorUtility.DisplayDialog("Mismatch", "Submesh count does not match material count.", "OK");
				return;
			}

			Undo.RegisterFullObjectHierarchyUndo(go, "Split Mesh By Material");

			for ( int i = 0; i < mesh.subMeshCount; i++ )
			{
				Mesh newMesh = new Mesh
				{
					name = mesh.name + "_Mat_" + i
				};

				newMesh.vertices = mesh.vertices;
				newMesh.normals = mesh.normals;
				newMesh.tangents = mesh.tangents;
				newMesh.uv = mesh.uv;
				newMesh.uv2 = mesh.uv2;
				newMesh.colors = mesh.colors;

				newMesh.triangles = mesh.GetTriangles(i);
				newMesh.RecalculateBounds();

				GameObject part = new GameObject(mesh.name + "_Part_" + i);
				Undo.RegisterCreatedObjectUndo(part, "Create split mesh");

				part.transform.SetParent(go.transform, false);

				MeshFilter partMF = part.AddComponent<MeshFilter>();
				MeshRenderer partMR = part.AddComponent<MeshRenderer>();

				partMF.sharedMesh = newMesh;
				partMR.sharedMaterial = mr.sharedMaterials[i];
			}

			// Disable original renderer (non-destructive)
			mr.enabled = false;
		}

		// Enables/disables the menu item automatically
		[MenuItem("Tools/Artificer/Mesh/Split Mesh By Material", true)]
		private static bool ValidateSplitSelected()
		{
			GameObject go = Selection.activeGameObject;
			if ( !go )
				return false;

			MeshFilter mf = go.GetComponent<MeshFilter>();
			MeshRenderer mr = go.GetComponent<MeshRenderer>();

			return mf != null && mr != null && mf.sharedMesh != null;
		}
	}
}