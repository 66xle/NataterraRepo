using System.Collections;
using UnityEngine;

namespace Artifice
{
	[ExecuteInEditMode]
	public class ShowNormals : MonoBehaviour
	{
		public float length = 1.0f;
		public Color color = Color.black;

		// Use this for initialization

		Vector3[] verts;
		Vector3[] normals;

		void Start()
		{
			MeshFilter mf = GetComponent<MeshFilter>();
			if ( mf )
			{
				Mesh mesh = mf.sharedMesh;

				verts = mesh.vertices;
				normals = mesh.normals;
			}
		}

		// Update is called once per frame
		void Update()
		{
			Matrix4x4 tm = transform.localToWorldMatrix;

			for ( int i = 0; i < verts.Length; i++ )
			{
				Vector3 p = tm.MultiplyPoint3x4(verts[i]);
				Vector3 n = tm.MultiplyVector(normals[i].normalized);

				Debug.DrawLine(p, p + (n * length), color);
			}
		}
	}
}