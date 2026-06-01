using UnityEngine;
using UnityEditor;

namespace Artifice
{
	public class MeshPivotToolWindow : EditorWindow
	{
		private Vector3 pivotOffset;
		private Vector3 pivotRotation;
		private bool applyToMeshAsset;

		[MenuItem("Tools/Artificer/Mesh Pivot Tool")]
		static void Open()
		{
			GetWindow<MeshPivotToolWindow>("Mesh Pivot Tool");
		}

		void OnGUI()
		{
			EditorGUILayout.LabelField("Selected Object", EditorStyles.boldLabel);

			GameObject go = Selection.activeGameObject;
			if ( !go || !go.GetComponent<MeshFilter>() )
			{
				EditorGUILayout.HelpBox("Select a GameObject with a MeshFilter.", MessageType.Info);
				return;
			}

			pivotOffset = EditorGUILayout.Vector3Field("Pivot Offset (Local)", pivotOffset);
			pivotRotation = EditorGUILayout.Vector3Field("Pivot Rotation (Degrees)", pivotRotation);

			applyToMeshAsset = EditorGUILayout.Toggle(
				new GUIContent("Apply To Mesh Asset",
				"If enabled, modifies the original mesh asset.\nOtherwise, creates an instance copy."),
				applyToMeshAsset
			);

			GUILayout.Space(10);

			if ( GUILayout.Button("Apply Pivot Changes") )
			{
				ApplyPivot(go);
			}


			if ( GUILayout.Button("Reset Fields") )
			{
				pivotOffset = Vector3.zero;
				pivotRotation = Vector3.zero;
			}
		}

		void ApplyPivot(GameObject go)
		{
			MeshFilter mf = go.GetComponent<MeshFilter>();
			Mesh originalMesh = mf.sharedMesh;

			if ( !originalMesh )
			{
				Debug.LogError("No mesh found.");
				return;
			}

			Mesh mesh;
			if ( applyToMeshAsset )
			{
				mesh = originalMesh;
			}
			else
			{
				mesh = Instantiate(originalMesh);
				mesh.name = originalMesh.name + "_PivotAdjusted";
				mf.sharedMesh = mesh;
			}

			Undo.RegisterCompleteObjectUndo(go, "Adjust Mesh Pivot");
			Undo.RegisterCompleteObjectUndo(mesh, "Adjust Mesh Pivot");

			Vector3[] vertices = mesh.vertices;

			Quaternion rotation = Quaternion.Euler(pivotRotation);

			for ( int i = 0; i < vertices.Length; i++ )
			{
				vertices[i] = rotation * (vertices[i] - pivotOffset);
			}

			mesh.vertices = vertices;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			// Compensate transform so world position stays the same
			go.transform.position += go.transform.TransformVector(pivotOffset);
			go.transform.rotation *= Quaternion.Inverse(rotation);

			EditorUtility.SetDirty(mesh);
			EditorUtility.SetDirty(go);

			Debug.Log("Pivot updated successfully.");
		}
	}
}