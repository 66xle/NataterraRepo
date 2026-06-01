using UnityEngine;
using UnityEditor;

namespace Artifice
{
	public class SkinnedToStaticMeshConverter
	{
		[MenuItem("Tools/Flatten Hierarchy Meshes Correctly")]
		private static void FlattenSelected()
		{
			if ( Selection.activeGameObject == null )
			{
				Debug.LogError("Select a root GameObject.");
				return;
			}

			GameObject root = Selection.activeGameObject;
			Undo.RegisterFullObjectHierarchyUndo(root, "Flatten Hierarchy Meshes");

			// Start recursion with identity matrix (root's local transform)
			ProcessTransform(root.transform, Matrix4x4.identity);

			AssetDatabase.SaveAssets();
			Debug.Log("Hierarchy flattened successfully.");
		}

		private static void ProcessTransform(Transform t, Matrix4x4 parentMatrix)
		{
			// Compute cumulative transform: parent * local
			Matrix4x4 cumulative = parentMatrix * Matrix4x4.TRS(t.localPosition, t.localRotation, t.localScale);

			// Process SkinnedMeshRenderer
			SkinnedMeshRenderer smr = t.GetComponent<SkinnedMeshRenderer>();
			if ( smr != null )
			{
				Mesh bakedMesh = new Mesh();
				smr.BakeMesh(bakedMesh); // bake bones into mesh

				ApplyMatrixToMesh(bakedMesh, cumulative);

				// Save asset
				bakedMesh.name = t.name + "_Baked";
				string path = $"Assets/{bakedMesh.name}.asset";
				AssetDatabase.CreateAsset(bakedMesh, AssetDatabase.GenerateUniqueAssetPath(path));

				Material[] mats = smr.sharedMaterials;
				Object.DestroyImmediate(smr);

				MeshFilter mf = t.gameObject.AddComponent<MeshFilter>();
				MeshRenderer mr = t.gameObject.AddComponent<MeshRenderer>();
				mf.sharedMesh = bakedMesh;
				mr.sharedMaterials = mats;
			}

			// Process normal MeshFilter
			MeshFilter meshFilter = t.GetComponent<MeshFilter>();
			if ( meshFilter != null && meshFilter.sharedMesh != null )
			{
				Mesh mesh = Object.Instantiate(meshFilter.sharedMesh);
				ApplyMatrixToMesh(mesh, cumulative);

				mesh.name = meshFilter.sharedMesh.name + "_Baked";
				string path = $"Assets/{mesh.name}.asset";
				AssetDatabase.CreateAsset(mesh, AssetDatabase.GenerateUniqueAssetPath(path));

				meshFilter.sharedMesh = mesh;
			}

			// Reset local transform but visually preserve mesh
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;

			// Recurse children
			foreach ( Transform child in t )
				ProcessTransform(child, cumulative);
		}

		private static void ApplyMatrixToMesh(Mesh mesh, Matrix4x4 matrix)
		{
			Vector3[] verts = mesh.vertices;
			Vector3[] norms = mesh.normals;

			for ( int i = 0; i < verts.Length; i++ )
				verts[i] = matrix.MultiplyPoint3x4(verts[i]);

			for ( int i = 0; i < norms.Length; i++ )
				norms[i] = matrix.MultiplyVector(norms[i]).normalized;

			mesh.vertices = verts;
			mesh.normals = norms;
			mesh.RecalculateBounds();
		}
	}
}