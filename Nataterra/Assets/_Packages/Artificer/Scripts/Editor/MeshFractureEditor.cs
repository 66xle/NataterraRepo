using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Artifice
{
	public class MeshFractureEditor : EditorWindow
	{
		GameObject target;
		int fractureCount = 6;
		int seed = 0;

		[MenuItem("Tools/Irregular Mesh Fracture")]
		static void Open()
		{
			GetWindow<MeshFractureEditor>("Mesh Fracture");
		}

		void OnGUI()
		{
			target = (GameObject)EditorGUILayout.ObjectField(
				"Target Object", target, typeof(GameObject), true);

			fractureCount = EditorGUILayout.IntSlider("Fracture Count", fractureCount, 1, 12);
			seed = EditorGUILayout.IntField("Random Seed", seed);

			if (GUILayout.Button("Fracture Mesh") && target)
			{
				Fracture();
			}
		}

		void Fracture()
		{
			Random.InitState(seed);

			MeshFilter mf = target.GetComponent<MeshFilter>();
			if (!mf || !mf.sharedMesh)
			{
				Debug.LogError("Target needs a MeshFilter");
				return;
			}

			List<Mesh> meshes = new List<Mesh> { mf.sharedMesh };

			for (int i = 0; i < fractureCount; i++)
			{
				List<Mesh> newMeshes = new List<Mesh>();

				foreach (var mesh in meshes)
				{
					Plane plane = RandomPlane(mesh.bounds);
					SplitMesh(mesh, plane, out Mesh a, out Mesh b);
					if (a) newMeshes.Add(a);
					if (b) newMeshes.Add(b);
				}

				meshes = newMeshes;
			}

			CreateAssembledObject(target, meshes);
			//SaveChunks(meshes);
		}

		Plane RandomPlane(Bounds b)
		{
			Vector3 normal = Random.onUnitSphere;
			Vector3 point = b.center + Random.insideUnitSphere * b.extents.magnitude * 0.3f;
			return new Plane(normal, point);
		}

		// ----------------------------------------------------
		// CORE SPLIT WITH CAPS
		// ----------------------------------------------------

		void SplitMesh(Mesh mesh, Plane plane, out Mesh meshA, out Mesh meshB)
		{
			List<Vector3> va = new(), vb = new();
			List<int> ta = new(), tb = new();

			List<Vector3> cutPoints = new();

			Vector3[] verts = mesh.vertices;
			int[] tris = mesh.triangles;

			for (int i = 0; i < tris.Length; i += 3)
			{
				int i0 = tris[i];
				int i1 = tris[i + 1];
				int i2 = tris[i + 2];

				Vector3 v0 = verts[i0];
				Vector3 v1 = verts[i1];
				Vector3 v2 = verts[i2];

				bool s0 = plane.GetSide(v0);
				bool s1 = plane.GetSide(v1);
				bool s2 = plane.GetSide(v2);

				if (s0 && s1 && s2)
					AddTri(va, ta, v0, v1, v2);
				else if (!s0 && !s1 && !s2)
					AddTri(vb, tb, v0, v1, v2);
				else
					CutTriangle(plane, v0, v1, v2, s0, s1, s2,
						va, ta, vb, tb, cutPoints);
			}

			Cap(plane, cutPoints, va, ta, true);
			Cap(plane, cutPoints, vb, tb, false);

			meshA = BuildMesh(va, ta);
			meshB = BuildMesh(vb, tb);
		}

		void CutTriangle(
			Plane plane,
			Vector3 v0, Vector3 v1, Vector3 v2,
			bool s0, bool s1, bool s2,
			List<Vector3> va, List<int> ta,
			List<Vector3> vb, List<int> tb,
			List<Vector3> cutPoints)
		{
			Vector3[] v = { v0, v1, v2 };
			bool[] s = { s0, s1, s2 };

			List<Vector3> front = new();
			List<Vector3> back = new();

			for (int i = 0; i < 3; i++)
			{
				int j = (i + 1) % 3;

				if (s[i]) front.Add(v[i]);
				else back.Add(v[i]);

				if (s[i] != s[j])
				{
					plane.Raycast(
						new Ray(v[i], v[j] - v[i]),
						out float enter);

					Vector3 hit = v[i] + (v[j] - v[i]).normalized * enter;
					front.Add(hit);
					back.Add(hit);
					cutPoints.Add(hit);
				}
			}

			Triangulate(front, va, ta);
			Triangulate(back, vb, tb);
		}

		void Cap(
			Plane plane,
			List<Vector3> points,
			List<Vector3> v,
			List<int> t,
			bool flip)
		{
			if (points.Count < 3) return;

			Vector3 center = Vector3.zero;
			foreach (var p in points) center += p;
			center /= points.Count;

			points.Sort((a, b) =>
				Vector3.SignedAngle(a - center, b - center, plane.normal).CompareTo(0));

			int start = v.Count;
			v.Add(center);

			foreach (var p in points)
				v.Add(p);

			for (int i = 1; i < points.Count; i++)
			{
				if (flip)
					t.AddRange(new[] { start, start + i, start + i + 1 });
				else
					t.AddRange(new[] { start, start + i + 1, start + i });
			}
		}

		void Triangulate(List<Vector3> poly, List<Vector3> v, List<int> t)
		{
			for (int i = 1; i < poly.Count - 1; i++)
				AddTri(v, t, poly[0], poly[i], poly[i + 1]);
		}

		void AddTri(List<Vector3> v, List<int> t, Vector3 a, Vector3 b, Vector3 c)
		{
			int i = v.Count;
			v.Add(a); v.Add(b); v.Add(c);
			t.Add(i); t.Add(i + 1); t.Add(i + 2);
		}

		Mesh BuildMesh(List<Vector3> v, List<int> t)
		{
			if (t.Count < 3) return null;

			Mesh m = new Mesh();
			m.vertices = v.ToArray();
			m.triangles = t.ToArray();
			m.RecalculateNormals();
			m.RecalculateBounds();
			return m;
		}

		void SaveChunks(List<Mesh> meshes)
		{
			string path = EditorUtility.SaveFolderPanel(
				"Save Fractured Meshes", "Assets", "");

			if (string.IsNullOrEmpty(path)) return;

			path = path.Replace(Application.dataPath, "Assets");

			for (int i = 0; i < meshes.Count; i++)
			{
				AssetDatabase.CreateAsset(
					meshes[i],
					$"{path}/Chunk_{i}.asset");
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		void CreateAssembledObject(GameObject original, List<Mesh> meshes)
{
    GameObject root = new GameObject(original.name + "_Fractured");
    Undo.RegisterCreatedObjectUndo(root, "Create Fractured Object");

    // Match original transform
    root.transform.position = original.transform.position;
    root.transform.rotation = original.transform.rotation;
    root.transform.localScale = original.transform.localScale;

    Material mat = original.GetComponent<MeshRenderer>().sharedMaterial;

    for (int i = 0; i < meshes.Count; i++)
    {
        GameObject chunk = new GameObject("Chunk_" + i);
        Undo.RegisterCreatedObjectUndo(chunk, "Create Chunk");

        chunk.transform.SetParent(root.transform, false);
        chunk.transform.localPosition = Vector3.zero;
        chunk.transform.localRotation = Quaternion.identity;
        chunk.transform.localScale = Vector3.one;

        MeshFilter mf = chunk.AddComponent<MeshFilter>();
        MeshRenderer mr = chunk.AddComponent<MeshRenderer>();

        mf.sharedMesh = meshes[i];
        mr.sharedMaterial = mat;
    }

    // Optional: hide original
    original.SetActive(false);

    Selection.activeGameObject = root;
}

	}
}