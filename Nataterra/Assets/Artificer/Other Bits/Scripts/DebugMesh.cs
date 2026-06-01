using UnityEngine;
using System.Collections;

namespace Artifice
{
	public class DebugMesh: MonoBehaviour
	{
		void Start()
		{
			MeshFilter mf = GetComponent<MeshFilter>();
			Mesh m = mf.sharedMesh;

			Vector4[] tangs = m.tangents;

			for ( int i = 0; i < tangs.Length; i++ )
			{
				Debug.Log("t " + tangs[i]);
			}
		}
	}
}