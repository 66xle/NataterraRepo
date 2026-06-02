using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Artifice
{
	public static class MeshSplitterHybrid
	{
		public struct SplitValues
		{
			public Transform		root;
			public Transform		tm;
			public Material[]		mats;
		}

		private static int PackTri(int submesh, int triIndex) => (submesh << 24) | triIndex;
		private static void UnpackTri(int packed, out int submesh, out int triIndex)
		{
			submesh = (packed >> 24) & 0xFF;
			triIndex = packed & 0xFFFFFF;
		}

		public static Bounds TransformBounds(this Bounds localBounds, Matrix4x4 localToWorld)
		{
			Vector3 center = localBounds.center;
			Vector3 extents = localBounds.extents;

			Vector3[] corners = new Vector3[8]
			{
				new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z),
				new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z),
				new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z),
				new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z),

				new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z),
				new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z),
				new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z),
				new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z),
			};

			for ( int i = 0; i < corners.Length; i++ )
				corners[i] = localToWorld.MultiplyPoint3x4(corners[i]);

			Bounds worldBounds = new Bounds(corners[0], Vector3.zero);
			for ( int i = 1; i < corners.Length; i++ )
				worldBounds.Encapsulate(corners[i]);

			return worldBounds;
		}

		public static Matrix4x4 WorldToLocalMatrix(Matrix4x4 worldMatrix, Transform relativeTo)
		{
			return relativeTo.worldToLocalMatrix * worldMatrix;
		}

#region Jobs

#if false
		[BurstCompile]
		struct ComputeCornerKeyJob : IJobParallelFor
		{
			[ReadOnly] public NativeArray<float3> positions;
			[ReadOnly] public NativeArray<float2> uv0;
			[ReadOnly] public NativeArray<float2> uv1;
			[ReadOnly] public NativeArray<float3> normals;
			[ReadOnly] public NativeArray<float4> colors;

			[ReadOnly] public NativeArray<int> trianglesAll;

			[ReadOnly] public bool  usePos;
			[ReadOnly] public bool  useUV0;
			[ReadOnly] public bool  useUV1;
			[ReadOnly] public bool  useNormal;
			[ReadOnly] public bool  useColor;

			[ReadOnly] public float posTol;
			[ReadOnly] public float uvTol;
			[ReadOnly] public float normalTol;
			[ReadOnly] public float colorTol;

			[WriteOnly] public NativeArray<int> outCornerKeys;

			public void Execute(int i)
			{
				unchecked
				{
					int vi = trianglesAll[i];
					int h = 17;

					if (usePos)
					{
						float3 p = positions[vi];
						h = h * 31 + (int)math.floor(p.x / posTol);
						h = h * 31 + (int)math.floor(p.y / posTol);
						h = h * 31 + (int)math.floor(p.z / posTol);
					}

					if (useUV0)
					{
						float2 uv = uv0[vi];
						h = h * 31 + (int)math.floor(uv.x / uvTol);
						h = h * 31 + (int)math.floor(uv.y / uvTol);
					}

					if (useUV1)
					{
						float2 uv = uv1[vi];
						h = h * 31 + (int)math.floor(uv.x / uvTol);
						h = h * 31 + (int)math.floor(uv.y / uvTol);
					}

					if (useNormal)
					{
						float3 n = normals[vi];
						h = h * 31 + (int)math.floor(n.x / normalTol);
						h = h * 31 + (int)math.floor(n.y / normalTol);
						h = h * 31 + (int)math.floor(n.z / normalTol);
					}

					if (useColor)
					{
						float4 c = colors[vi];
						h = h * 31 + (int)math.floor(c.x / colorTol);
						h = h * 31 + (int)math.floor(c.y / colorTol);
						h = h * 31 + (int)math.floor(c.z / colorTol);
						h = h * 31 + (int)math.floor(c.w / colorTol);
					}

					outCornerKeys[i] = h;
				}
			}
		}	
#endif





		[BurstCompile]
		struct ComputeVertexKeyJob : IJobParallelFor
		{
			[ReadOnly] public NativeArray<float3>	positions;
			[ReadOnly] public NativeArray<float2>	uv0;
			[ReadOnly] public NativeArray<float2>	uv1;
			[ReadOnly] public NativeArray<float3>	normals;
			[ReadOnly] public NativeArray<float4>	colors;
			[ReadOnly] public bool					usePos;
			[ReadOnly] public bool					useUV0;
			[ReadOnly] public bool					useUV1;
			[ReadOnly] public bool					useNormal;
			[ReadOnly] public bool					useColor;
			[ReadOnly] public float					posTol;
			[ReadOnly] public float					uvTol;
			[ReadOnly] public float					normalTol;
			[ReadOnly] public float					colorTol;
			[WriteOnly] public NativeArray<int>		outKeys;

			public void Execute(int i)
			{
				unchecked
				{
					int h = 17;
					if ( usePos )
					{
						h = h * 31 + (int)math.floor(positions[i].x / posTol);
						h = h * 31 + (int)math.floor(positions[i].y / posTol);
						h = h * 31 + (int)math.floor(positions[i].z / posTol);
					}
					if ( useUV0 )
					{
						h = h * 31 + (int)math.floor(uv0[i].x / uvTol);
						h = h * 31 + (int)math.floor(uv0[i].y / uvTol);
					}
					if ( useUV1 )
					{
						h = h * 31 + (int)math.floor(uv1[i].x / uvTol);
						h = h * 31 + (int)math.floor(uv1[i].y / uvTol);
					}
					if ( useNormal )
					{
						h = h * 31 + (int)math.floor(normals[i].x / normalTol);
						h = h * 31 + (int)math.floor(normals[i].y / normalTol);
						h = h * 31 + (int)math.floor(normals[i].z / normalTol);
					}
					if ( useColor )
					{
						h = h * 31 + (int)math.floor(colors[i].x / colorTol);
						h = h * 31 + (int)math.floor(colors[i].y / colorTol);
						h = h * 31 + (int)math.floor(colors[i].z / colorTol);
						h = h * 31 + (int)math.floor(colors[i].w / colorTol);
					}
					outKeys[i] = h;
				}
			}
		}

		[BurstCompile]
		struct BuildAdjacencyJob : IJob
		{
			[ReadOnly] public NativeArray<int> vertexKeys;
			[ReadOnly] public NativeArray<int> trianglesAll;
			[ReadOnly] public NativeArray<int> submeshOffsets;
			[ReadOnly] public NativeArray<int> submeshTriCounts;
			public NativeParallelMultiHashMap<int, int>.ParallelWriter adjacencyWriter;

			public void Execute()
			{
				int subMeshCount = submeshOffsets.Length;
				for ( int s = 0; s < subMeshCount; s++ )
				{
					int triCount = submeshTriCounts[s];
					int baseIntIndex = submeshOffsets[s];
					for ( int tri = 0; tri < triCount; tri++ )
					{
						int tInt = baseIntIndex + tri * 3;
						for ( int v = 0; v < 3; v++ )
						{
							int vertexIndex = trianglesAll[tInt + v];
							int key = vertexKeys[vertexIndex];
							int packedTri = PackTri(s, tri);
							adjacencyWriter.Add(key, packedTri);
						}
					}
				}
			}
		}

#if false
		[BurstCompile]
		struct BuildAdjacencyCornerJob : IJob
		{
			[ReadOnly] public NativeArray<int> cornerKeys;
			[ReadOnly] public NativeArray<int> trianglesAll;
			[ReadOnly] public NativeArray<int> submeshOffsets;
			[ReadOnly] public NativeArray<int> submeshTriCounts;

			public NativeParallelMultiHashMap<int, int>.ParallelWriter adjacencyWriter;

			public void Execute()
			{
				int subMeshCount = submeshOffsets.Length;

				for (int s = 0; s < subMeshCount; s++)
				{
					int triCount = submeshTriCounts[s];
					int baseIntIndex = submeshOffsets[s];

					for (int tri = 0; tri < triCount; tri++)
					{
						int tInt = baseIntIndex + tri * 3;
						int packedTri = PackTri(s, tri);

						adjacencyWriter.Add(cornerKeys[tInt + 0], packedTri);
						adjacencyWriter.Add(cornerKeys[tInt + 1], packedTri);
						adjacencyWriter.Add(cornerKeys[tInt + 2], packedTri);
					}
				}
			}
		}
#endif
#if false
		[BurstCompile]
		struct BuildAdjacencyFromEdgesJob : IJob
		{
			[ReadOnly] public NativeArray<int> cornerKeys;
			[ReadOnly] public NativeArray<int> submeshOffsets;
			[ReadOnly] public NativeArray<int> submeshTriCounts;

			public NativeParallelMultiHashMap<long, int> edgeMap;
			public NativeParallelMultiHashMap<int, int>.ParallelWriter adjacencyWriter;

			public void Execute()
			{
				int subMeshCount = submeshOffsets.Length;

				// --- 1. Emit edges ---
				for (int s = 0; s < subMeshCount; s++)
				{
					int triCount = submeshTriCounts[s];
					int baseInt = submeshOffsets[s];

					for (int tri = 0; tri < triCount; tri++)
					{
						int t = baseInt + tri * 3;
						int packedTri = PackTri(s, tri);

						EmitEdge(cornerKeys[t + 0], cornerKeys[t + 1], packedTri);
						EmitEdge(cornerKeys[t + 1], cornerKeys[t + 2], packedTri);
						EmitEdge(cornerKeys[t + 2], cornerKeys[t + 0], packedTri);
					}
				}

				// --- 2. Convert shared edges → triangle adjacency ---
				var keys = edgeMap.GetKeyArray(Allocator.Temp);
				var values = edgeMap.GetValueArray(Allocator.Temp);

				for (int i = 0; i < keys.Length; i++)
				{
					long edge = keys[i];
					int triA = values[i];

					NativeParallelMultiHashMapIterator<long> it;
					int triB;

					if (edgeMap.TryGetFirstValue(edge, out triB, out it))
					{
						do
						{
							if (triA != triB)
								adjacencyWriter.Add(triA, triB);
						}
						while (edgeMap.TryGetNextValue(out triB, ref it));
					}
				}

				keys.Dispose();
				values.Dispose();
			}

			void EmitEdge(int a, int b, int tri)
			{
				int min = math.min(a, b);
				int max = math.max(a, b);
				long edgeKey = ((long)min << 32) | (uint)max;
				edgeMap.Add(edgeKey, tri);
			}
		}
#endif



		#endregion

#if true
		//public static List<MeshElement> SplitMeshMat(Artificer artificer, Matrix4x4 meshtm, Mesh sourceMesh, SplitValues opts, SplitOptions so, MeshRenderer mr)
		public static List<MeshElement> SplitMeshMat(Artificer artificer, Matrix4x4 meshtm, Mesh sourceMesh, SplitValues opts, SplitOptions so, Renderer mr, int mindex)
		{
			if ( sourceMesh == null )
				return null;

			List<MeshElement>	meshElements = new List<MeshElement>();

			int subMeshCount = sourceMesh.subMeshCount;
			Mesh[] result = new Mesh[subMeshCount];

			// Get original data
			Vector3[] allVerts		= sourceMesh.vertices;
			Vector3[] allNormals	= sourceMesh.normals;
			Vector4[] allTangents	= sourceMesh.tangents;
			Vector2[] allUV			= sourceMesh.uv;
			Vector2[] allUV2		= sourceMesh.uv2;
			Color[] allColors		= sourceMesh.colors;

			// We need to get normals, tangents and verts into the local space of the root
			for ( int i = 0; i < allVerts.Length; i++ )
			{
				allVerts[i] = meshtm.MultiplyPoint(allVerts[i]);
				allNormals[i] = meshtm.MultiplyVector(allNormals[i]);
				float w = allTangents[i].w;
				allTangents[i] = meshtm.MultiplyVector(allTangents[i]);
				allTangents[i].w = w;
			}

			Bounds sourceBounds = sourceMesh.bounds;

			for ( int s = 0; s < subMeshCount; s++ )
			{
				int[] triangles = sourceMesh.GetTriangles(s);

				if ( triangles.Length > 0 )
				{
					//float sortMod = so.sortModifier.value;	//opts.sortModifier;
					//BuildStyle bstyle = BuildStyle.None;
					//MeshPivotMode pivotMode = so.meshPivot.value;	//opts.artificer.meshPivot;

					//SplitParams sp = null;
					SplitOptions defso = so;
#if false
					if ( artificer.useSplitParams )
					{
						sp = opts.tm.GetComponent<SplitParams>();
						if ( sp )
						{
							defso = new SplitOptions();
							SplitOptions.Make(ref defso, so, sp.splitOptions);
							//pivotMode = sp.meshPivot;
							//sortMod = sp.sortModifier;
							//bstyle = sp.buildStyle;

							//if ( sp.useSortSpline && sp.sortSpline )
								//opts.spline = sp.sortSpline;
						}
					}
#endif
					MeshElement me = new MeshElement();

					// Map from old vertex index → new vertex index
					Dictionary<int, int> vertMap = new Dictionary<int, int>();
					List<Vector3>	verts	= new List<Vector3>();
					List<Vector3>	norms	= new List<Vector3>();
					List<Vector4>	tangs	= new List<Vector4>();
					List<Vector2>	uv		= new List<Vector2>();
					List<Vector2>	uv2		= new List<Vector2>();
					List<Color>		cols	= new List<Color>();
					List<int>		newTris	= new List<int>();

					Vector3 center = Vector3.zero;

					for ( int i = 0; i < triangles.Length; i++ )
					{
						int oldIndex = triangles[i];
						int newIndex;

						if ( !vertMap.TryGetValue(oldIndex, out newIndex) )
						{
							newIndex = verts.Count;
							vertMap.Add(oldIndex, newIndex);

							verts.Add(allVerts[oldIndex]);
							center += allVerts[oldIndex];

							if ( allNormals != null && allNormals.Length > oldIndex )
								norms.Add(allNormals[oldIndex]);

							if ( allTangents != null && allTangents.Length > oldIndex )
								tangs.Add(allTangents[oldIndex]);

							if ( allUV != null && allUV.Length > oldIndex )
								uv.Add(allUV[oldIndex]);

							if ( allUV2 != null && allUV2.Length > oldIndex )
								uv2.Add(allUV2[oldIndex]);

							if ( allColors != null && allColors.Length > oldIndex )
								cols.Add(allColors[oldIndex]);
						}

						newTris.Add(newIndex);
					}

					me.center = center / verts.Count;
					me.bounds = new Bounds();

					// Change pivot
					Matrix4x4 pivotTM = Matrix4x4.identity;

					switch ( defso.meshPivot.value )
					{
						case MeshPivotMode.Object:
							for ( int v = 0; v < verts.Count; v++ )
								me.bounds.Encapsulate(verts[v]);
							break;

						case MeshPivotMode.Center:
							for ( int v = 0; v < verts.Count; v++ )
							{
								me.bounds.Encapsulate(verts[v]);
								verts[v] -= me.center;
							}

							pivotTM = Matrix4x4.Translate(me.center);
							break;

						case MeshPivotMode.Bottom:
#if false
							for ( int v = 0; v < verts.Count; v++ )
							{
								Vector3 vp = verts[v];
								me.bounds.Encapsulate(verts[v]);
								//vp -= me.center;
								vp.y -= sourceBounds.min.y;
								verts[v] = vp;
							}
							pivotTM = Matrix4x4.Translate(new Vector3(0.0f, sourceBounds.min.y, 0.0f));
#endif
							for ( int v = 0; v < verts.Count; v++ )
							{
								Vector3 vp = verts[v];
								vp -= me.center;
								verts[v] = vp;
								me.bounds.Encapsulate(verts[v]);
							}

							Vector3 bsize = me.bounds.size;
							me.bounds.size = Vector3.zero;
							for ( int v = 0; v < verts.Count; v++ )
							{
								Vector3 vp = verts[v];
								vp.y += bsize.y * 0.5f;
								verts[v] = vp;
								me.bounds.Encapsulate(verts[v]);
							}

							pivotTM = Matrix4x4.Translate(new Vector3(me.center.x, me.center.y - (bsize.y * 0.5f), me.center.z));

							break;

						case MeshPivotMode.Top:
#if false
							for ( int v = 0; v < verts.Count; v++ )
							{
								Vector3 vp = verts[v];
								me.bounds.Encapsulate(verts[v]);
								vp.y -= sourceBounds.max.y;
								verts[v] = vp;
							}
							pivotTM = Matrix4x4.Translate(new Vector3(0.0f, sourceBounds.max.y, 0.0f));
#endif
							for ( int v = 0; v < verts.Count; v++ )
							{
								Vector3 vp = verts[v];
								vp -= me.center;
								verts[v] = vp;
								me.bounds.Encapsulate(verts[v]);
							}

							Vector3 bsize1 = me.bounds.size;
							me.bounds.size = Vector3.zero;
							for ( int v = 0; v < verts.Count; v++ )
							{
								Vector3 vp = verts[v];
								vp.y -= bsize1.y * 0.5f;
								verts[v] = vp;
								me.bounds.Encapsulate(verts[v]);
							}

							pivotTM = Matrix4x4.Translate(new Vector3(me.center.x, me.center.y + (bsize1.y * 0.5f), me.center.z));
							break;
					}

					me.verts		= verts.ToArray();
					me.normals		= norms.ToArray();
					me.tangents		= tangs.ToArray();
					me.uvs			= uv.ToArray();
					me.uv2			= uv2.ToArray();
					me.colors		= cols.ToArray();
					me.subMeshCount	= 1;	//subMeshCount;
					me.tris			= new MeshTris[1];
					me.draw			= new List<int>();
					me.draw.Add(0);
					me.tris[0]		= new MeshTris();
					me.tris[0].tris = newTris.ToArray();
					me.tm1			= opts.tm.localToWorldMatrix * pivotTM;
					//me.tm1		= opts.tm.localToWorldMatrix;	// * pivotTM;
					me.position		= opts.tm.localPosition; //Vector3.zero;
					me.rotation		= opts.tm.localRotation; //Quaternion.identity;
					me.scale		= opts.tm.localScale;
					me.mats			= new Material[1];
					me.mats[0]		= opts.mats[s];
					me.tm			= WorldToLocalMatrix(me.tm1, opts.root);
					me.axisPoint	= new Vector3(0.0f, me.center.y, 0.0f);
					//me.bounds		= TransformBounds(mesh.bounds, me.tm1);

					if ( defso.sortSpline.value && defso.useSortSpline.value )
					{
						float3 point = defso.sortSpline.value.transform.InverseTransformPoint(opts.tm.TransformPoint(me.center));
						float3 nearest;

						float alpha;
						float dist = SplineUtility.GetNearestPoint<Spline>(defso.sortSpline.value.Spline, point, out nearest, out alpha);

						point = opts.tm.InverseTransformPoint(defso.sortSpline.value.transform.TransformPoint(nearest));

						switch ( so.sortDistanceMode.value )
						{
							case SortDistanceMode.Closest:
								me.closestPoint = me.bounds.ClosestPoint(point);    //tm.InverseTransformPoint(point));
								break;

							case SortDistanceMode.Furthest:
								me.closestPoint = me.bounds.FurthestPoint(point);    //tm.InverseTransformPoint(point));
								break;

							case SortDistanceMode.Centre:
								me.closestPoint = me.bounds.center;    //tm.InverseTransformPoint(point));
								break;
						}
						//me.closestPoint = me.bounds.ClosestPoint(point);    //tm.InverseTransformPoint(point));
						me.distance = (dist + (alpha * defso.sortPathBias.value)) + defso.sortModifier.value;	//sortMod;
					}
					else
					{
						switch ( so.sortDistanceMode.value )
						{
							case SortDistanceMode.Closest:
								me.closestPoint = me.center + me.bounds.ClosestPoint(so.sortOrigin.value);    //tm.InverseTransformPoint(point));
								break;

							case SortDistanceMode.Furthest:
								me.closestPoint = me.center + me.bounds.FurthestPoint(so.sortOrigin.value);    //tm.InverseTransformPoint(point));
								break;

							case SortDistanceMode.Centre:
								me.closestPoint = me.center + me.bounds.center;    //tm.InverseTransformPoint(point));
								break;
						}

						//me.closestPoint = me.center + me.bounds.ClosestPoint(so.sortOrigin.value);	//opts.sortOrigin);
						if ( defso.sortMode.value == SortMode.Random )
						{
							me.distance = artificer.RandomValue() + defso.sortModifier.value;
						}
						else
							me.distance = Vector3.Distance(me.closestPoint, so.sortOrigin.value) + defso.sortModifier.value;
					}

					//me.buildStyle = bstyle;
					//me.splitParams = sp;
					so.Copy(me, artificer);

					Vector3 dir = (me.center - me.origin);  // - me.center);  //.normalized;
					if ( dir.sqrMagnitude.Equals(0.0f) )
						dir = Vector3.up;
					dir.x *= me.projection.x;
					dir.y *= me.projection.y;
					dir.z *= me.projection.z;
					me.dir = dir.normalized;	// * me.buildDist * (1.0f - a));

					//if ( defso.buildFrom.value != null )	//&& defso.buildFrom.value.Length > 0 && defso.buildFrom.value[0] )	//artificer.buildFrom )
					{
						//artificer.CalcSpline(me, artificer.buildFrom, artificer.startTension, artificer.endTension);
						artificer.CalcSpline(me, defso, opts.tm);	//defso.buildFrom.value, defso.startTension.value, defso.endTension.value);
					}

					//Debug.Log("mat " + me.mats[0]);
					me.matSortID = artificer.GetMatSortID(me.mats[0]);
					//me.mr = opts.tm.GetComponent<MeshRenderer>();
					//me.gameObject = opts.tm.name;	// TODO: dont need
					me.renderer = mr;
#if UNITY_6000_3_OR_NEWER
					me.id = mr.gameObject.GetEntityId();
#else
					me.id = mr.gameObject.GetInstanceID();
#endif
					me.id = mr.GetComponent<ObjectID>().id;

					//me.index = meshElements.Count;

					meshElements.Add(me);
				}
			}

			return meshElements;
		}
#endif

		//public static List<MeshElement> SplitMesh(Artificer artificer, Matrix4x4 meshtm, Mesh source, SplitValues opts, SplitOptions so, MeshRenderer mr)
		public static List<MeshElement> SplitMesh(Artificer artificer, Matrix4x4 meshtm, Mesh source, SplitValues opts, SplitOptions so, Renderer mr, int mindex)
		{
			SplitOptions baseso = so;

			if ( source == null || source.vertexCount == 0 )
				return null;

			if ( artificer.splitMode == SplitMode.Materials )
				return SplitMeshMat(artificer, meshtm, source, opts, so, mr, mindex);

			int vertexCount = source.vertexCount;
			Vector3[] vertices = source.vertices;

			Vector2[] uv0;
			if ( source.uv != null && source.uv.Length > 0 )
				uv0 = source.uv;
			else
				uv0 = new Vector2[vertexCount];

			Vector2[] uv1;
			if ( source.uv2 != null && source.uv2.Length > 0 )
				uv1 = source.uv2;
			else
				uv1 = new Vector2[vertexCount];

			Vector2[] uv3;
			if ( source.uv3 != null && source.uv3.Length > 0 )
				uv3 = source.uv3;
			else
				uv3 = new Vector2[vertexCount];

			Vector2[] uv4;
			if ( source.uv4 != null && source.uv4.Length > 0 )
				uv4 = source.uv4;
			else
				uv4 = new Vector2[vertexCount];

			Vector4[] tangents;
			if ( source.tangents != null && source.tangents.Length > 0 )
				tangents = source.tangents;
			else
				tangents = new Vector4[vertexCount];

			Vector3[] normals = source.normals ?? new Vector3[vertexCount];
			Color[] colors = null;
			if ( source.colors != null && source.colors.Length == vertexCount ) colors = source.colors;
			else if ( source.colors32 != null && source.colors32.Length == vertexCount )
			{
				colors = new Color[vertexCount];
				for ( int i = 0; i < vertexCount; i++ ) colors[i] = source.colors32[i];
			}
			else colors = new Color[vertexCount];

			Matrix4x4 M = meshtm;	//transform.localToWorldMatrix;
			Matrix4x4 N = M.inverse.transpose;

			for ( int i = 0; i < vertices.Length; i++ )
			{
				vertices[i]	= meshtm.MultiplyPoint(vertices[i]);
				//normals[i]	= meshtm.MultiplyVector(normals[i]);
				normals[i]	= N.MultiplyVector(normals[i]);

				Vector3 t = new Vector3(tangents[i].x, tangents[i].y, tangents[i].z);

				float w = tangents[i].w;

				//tangents[i]	= meshtm.MultiplyVector(tangents[i]);
				tangents[i]	= N.MultiplyVector(t).normalized;
				tangents[i].w = w;
			}

			int subMeshCount = source.subMeshCount;
			List<int> trianglesAllList = new List<int>();
			int[] triCounts = new int[subMeshCount];
			int[] submeshIntOffsets = new int[subMeshCount];
			for ( int s = 0; s < subMeshCount; s++ )
			{
				int[] tri = source.GetTriangles(s);
				submeshIntOffsets[s] = trianglesAllList.Count;
				triCounts[s] = tri.Length / 3;
				trianglesAllList.AddRange(tri);
			}

			// Create native arrays for jobs
			var nPositions	= new NativeArray<float3>(vertexCount, Allocator.TempJob);
			var nUV0		= new NativeArray<float2>(vertexCount, Allocator.TempJob);
			var nUV1		= new NativeArray<float2>(vertexCount, Allocator.TempJob);
			var nNormals	= new NativeArray<float3>(vertexCount, Allocator.TempJob);
			var nColors		= new NativeArray<float4>(vertexCount, Allocator.TempJob);

			for ( int i = 0; i < vertexCount; i++ )
			{
				nPositions[i] = vertices[i];
				nUV0[i] = uv0[i];
				nUV1[i] = uv1[i];
				nNormals[i] = normals[i];
				Color c = colors != null ? colors[i] : Color.clear;
				nColors[i] = new float4(c.r, c.g, c.b, c.a);
			}

			var nVertexKeys = new NativeArray<int>(vertexCount, Allocator.TempJob);

			var nTrianglesAll = new NativeArray<int>(trianglesAllList.Count, Allocator.TempJob);
			for ( int i = 0; i < trianglesAllList.Count; i++ ) nTrianglesAll[i] = trianglesAllList[i];

			var nSubmeshOffsets = new NativeArray<int>(submeshIntOffsets.Length, Allocator.TempJob);
			for ( int i = 0; i < submeshIntOffsets.Length; i++ ) nSubmeshOffsets[i] = submeshIntOffsets[i];

			var nSubmeshTriCounts = new NativeArray<int>(triCounts.Length, Allocator.TempJob);
			for ( int i = 0; i < triCounts.Length; i++ ) nSubmeshTriCounts[i] = triCounts[i];

			var adjacency = new NativeParallelMultiHashMap<int, int>(math.max(1, trianglesAllList.Count), Allocator.TempJob);

			// Schedule jobs
			var computeJob = new ComputeVertexKeyJob
			{
				positions	= nPositions,
				uv0			= nUV0,
				uv1			= nUV1,
				normals		= nNormals,
				colors		= nColors,
				usePos		= artificer.usePosition,
				useUV0		= artificer.useUV,
				useUV1		= artificer.useUV2,
				useNormal	= artificer.useNormal,
				useColor	= artificer.useColor,
				posTol		= math.max(1e-9f, artificer.positionTolerance),
				uvTol		= math.max(1e-9f, artificer.uvTolerance),
				normalTol	= math.max(1e-9f, artificer.normalTolerance),
				colorTol	= math.max(1e-6f, artificer.colorTolerance),
				outKeys		= nVertexKeys
			};
			var computeHandle = computeJob.Schedule(vertexCount, 64);

			var buildAdj = new BuildAdjacencyJob
			{
				vertexKeys			= nVertexKeys,
				trianglesAll		= nTrianglesAll,
				submeshOffsets		= nSubmeshOffsets,
				submeshTriCounts	= nSubmeshTriCounts,
				adjacencyWriter		= adjacency.AsParallelWriter()
			};

			var buildHandle = buildAdj.Schedule(computeHandle);
			buildHandle.Complete();

			// BFS connectivity on main thread
			var subTriangles = new int[subMeshCount][];
			for ( int s = 0; s < subMeshCount; s++ )
				subTriangles[s] = source.GetTriangles(s);

			var canonical = new int[vertexCount];
			var keyToCanonical = new Dictionary<int, int>(vertexCount);
			int nextCanonical = 0;

			for ( int i = 0; i < vertexCount; i++ )
			{
				int key = nVertexKeys[i];
				if ( !keyToCanonical.TryGetValue(key, out int cid) )
				{
					cid = nextCanonical++;
					keyToCanonical[key] = cid;
				}
				canonical[i] = cid;
			}

			// BFS to find components
			var visited			= new HashSet<int>();
			var components		= new List<List<int>>();
			var meshElements	= new List<MeshElement>();
			var usedMats		= new List<Material>();

			for ( int s = 0; s < subMeshCount; s++ )
			{
				int triCount = triCounts[s];

				for ( int tri = 0; tri < triCount; tri++ )
				{
					int packed = PackTri(s, tri);
					if ( visited.Contains(packed) ) continue;

					var queue = new Queue<int>();
					var comp = new List<int>();
					queue.Enqueue(packed);
					visited.Add(packed);

					while ( queue.Count > 0 )
					{
						int curPacked = queue.Dequeue();
						comp.Add(curPacked);

						UnpackTri(curPacked, out int cs, out int ctri);
						int[] triArr = subTriangles[cs];
						int baseIdx = ctri * 3;

						for ( int v = 0; v < 3; v++ )
						{
							int vi = triArr[baseIdx + v];
							int vKey = nVertexKeys[vi];

							if ( adjacency.TryGetFirstValue(vKey, out int neighborPacked, out var it) )
							{
								do
								{
									if ( !visited.Contains(neighborPacked) )
									{
										if ( !artificer.useMaterial )
										{
											visited.Add(neighborPacked);
											queue.Enqueue(neighborPacked);
										}
										else
										{
											UnpackTri(neighborPacked, out int nsub, out int ntri);
											if ( nsub == cs )
											{
												visited.Add(neighborPacked);
												queue.Enqueue(neighborPacked);
											}
										}
									}
								} while ( adjacency.TryGetNextValue(out neighborPacked, ref it) );
							}
						}
					}

					components.Add(comp);
				}
			}

			// Build meshes
			Bounds sourceBounds = source.bounds;
			for ( int ci = 0; ci < components.Count; ci++ )
			{
				var comp			= components[ci];
				var newVerts		= new List<Vector3>();
				var newUV0			= new List<Vector2>();
				var newUV1			= new List<Vector2>();
				var newUV3			= new List<Vector2>();
				var newUV4			= new List<Vector2>();
				var newNormals		= new List<Vector3>();
				var newTangents		= new List<Vector4>();
				var newColorsOut	= new List<Color>();
				var vertMap			= new Dictionary<int, int>();
				var outSubLists		= new List<int>[subMeshCount];
				for ( int s = 0; s < subMeshCount; s++ ) outSubLists[s] = new List<int>();

				Vector3 center = Vector3.zero;

				foreach ( int packed in comp )
				{
					UnpackTri(packed, out int ssub, out int stri);
					int[] triArr = subTriangles[ssub];
					int baseIdx = stri * 3;

					for ( int v = 0; v < 3; v++ )
					{
						int oldVi = triArr[baseIdx + v];
						if ( !vertMap.TryGetValue(oldVi, out int newVi) )
						{
							Vector3 vp = vertices[oldVi];
							newVi = newVerts.Count;
							vertMap[oldVi] = newVi;
							newVerts.Add(vp);
							center += vp;

							if ( uv0 != null && uv0.Length > oldVi ) newUV0.Add(uv0[oldVi]);
							if ( uv1 != null && uv1.Length > oldVi ) newUV1.Add(uv1[oldVi]);
							if ( uv3 != null && uv3.Length > oldVi ) newUV3.Add(uv3[oldVi]);
							if ( uv4 != null && uv4.Length > oldVi ) newUV4.Add(uv4[oldVi]);
							if ( normals != null && normals.Length > oldVi ) newNormals.Add(normals[oldVi]);
							if ( tangents != null && tangents.Length > oldVi ) newTangents.Add(tangents[oldVi]);
							if ( colors != null && colors.Length > oldVi ) newColorsOut.Add(colors[oldVi]);
						}
						outSubLists[ssub].Add(newVi);
					}
				}

				MeshElement me = new MeshElement();

				me.center = center / newVerts.Count;
				me.bounds = new Bounds();

				//float sortMod = opts.sortModifier;
				//BuildStyle bstyle = BuildStyle.None;
				//MeshPivotMode pivotMode = opts.artificer.meshPivot;

				//SplitOptions so = opts.artificer.splitOptions;
				SplitOptions defso = so;

#if false
				SplitParams sp = null;

				if ( artificer.useSplitParams )	//opts.useParams )
				{
					Debug.Log("opts tm " + opts.tm.name);
					sp = opts.tm.GetComponent<SplitParams>();
					if ( sp )
					{
						//pivotMode = sp.meshPivot;
						//sortMod = sp.sortModifier;
						//bstyle = sp.buildStyle;

						//if ( sp.useSortSpline && sp.sortSpline )
							//opts.spline = sp.sortSpline;

						defso = new SplitOptions();
						SplitOptions.Make(ref defso, so, sp.splitOptions);

						Debug.Log("Split params " + defso.buildFrom.value);
					}
				}
#endif

				// Change pivot
				Matrix4x4 pivotTM = Matrix4x4.identity;

				switch ( defso.meshPivot.value )	//pivotMode )
				{
					case MeshPivotMode.Object:
						for ( int v = 0; v < newVerts.Count; v++ )
							me.bounds.Encapsulate(newVerts[v]);
						break;

					case MeshPivotMode.Center:
						for ( int v = 0; v < newVerts.Count; v++ )
						{
							newVerts[v] -= me.center;
							me.bounds.Encapsulate(newVerts[v]);
						}

						pivotTM = Matrix4x4.Translate(me.center);
						break;

					case MeshPivotMode.Bottom:
#if false
						for ( int v = 0; v < newVerts.Count; v++ )
						{
							Vector3 vp = newVerts[v];
							me.bounds.Encapsulate(newVerts[v]);
							vp.y -= sourceBounds.min.y;
							newVerts[v] = vp;
						}
						pivotTM = Matrix4x4.Translate(new Vector3(0.0f, sourceBounds.min.y, 0.0f));
#endif
						for ( int v = 0; v < newVerts.Count; v++ )
						{
							Vector3 vp = newVerts[v];
							vp -= me.center;
							newVerts[v] = vp;
							me.bounds.Encapsulate(newVerts[v]);
						}

						Vector3 bsize = me.bounds.size;
						me.bounds.size = Vector3.zero;
						for ( int v = 0; v < newVerts.Count; v++ )
						{
							Vector3 vp = newVerts[v];
							vp.y += bsize.y * 0.5f;
							newVerts[v] = vp;
							me.bounds.Encapsulate(newVerts[v]);
						}

						pivotTM = Matrix4x4.Translate(new Vector3(me.center.x, me.center.y - (bsize.y * 0.5f), me.center.z));
						break;

					case MeshPivotMode.Top:
#if false
						for ( int v = 0; v < newVerts.Count; v++ )
						{
							Vector3 vp = newVerts[v];
							me.bounds.Encapsulate(newVerts[v]);
							vp.y -= sourceBounds.max.y;
							newVerts[v] = vp;
						}
						pivotTM = Matrix4x4.Translate(new Vector3(0.0f, sourceBounds.max.y, 0.0f));
#endif
						for ( int v = 0; v < newVerts.Count; v++ )
						{
							Vector3 vp = newVerts[v];
							vp -= me.center;
							newVerts[v] = vp;
							me.bounds.Encapsulate(newVerts[v]);
						}

						Vector3 bsize1 = me.bounds.size;
						me.bounds.size = Vector3.zero;
						for ( int v = 0; v < newVerts.Count; v++ )
						{
							Vector3 vp = newVerts[v];
							vp.y -= bsize1.y * 0.5f;
							newVerts[v] = vp;
							me.bounds.Encapsulate(newVerts[v]);
						}

						pivotTM = Matrix4x4.Translate(new Vector3(me.center.x, me.center.y + (bsize1.y * 0.5f), me.center.z));

						break;
				}

				me.subMeshCount	= subMeshCount;
				me.verts		= newVerts.ToArray();
				me.uvs			= newUV0.ToArray();
				me.uv2			= newUV1.ToArray();
				me.normals		= newNormals.ToArray();
				me.tangents		= newTangents.ToArray();
				me.colors		= newColorsOut.ToArray();
				me.tris			= new MeshTris[subMeshCount];
				me.draw			= new List<int>();

				//Debug.Log("verts " + newVerts.Count);
				usedMats.Clear();
				for ( int s = 0; s < subMeshCount; s++ )
				{
					me.tris[s] = new MeshTris();
					if ( outSubLists[s].Count > 0 )
					{
						me.draw.Add(s);
						me.tris[s].tris = outSubLists[s].ToArray();
						usedMats.Add(opts.mats[s]);
					}
					else
						me.tris[s].tris = Array.Empty<int>();
				}

				me.tm1			= opts.root.localToWorldMatrix * pivotTM;	//Matrix4x4.identity;	//pivotTM;	//opts.tm.localToWorldMatrix * pivotTM;
				//me.tm1		= opts.tm.localToWorldMatrix;
				me.position		= opts.tm.localPosition; //Vector3.zero;
				me.rotation		= opts.tm.localRotation; //Quaternion.identity;
				me.scale		= opts.tm.localScale;
				me.mats			= opts.mats;
				me.tm			= WorldToLocalMatrix(me.tm1, opts.root);	// opts.root.localToWorldMatrix;	//.worldToLocalMatrix;	//WorldToLocalMatrix(me.tm1, opts.root);
				me.axisPoint	= new Vector3(0.0f, me.center.y, 0.0f);
				//me.bounds		= TransformBounds(mesh.bounds, me.tm1);

				if ( artificer.useSplitParams )
				{
					//Debug.Log("me " + (artificer.buildData.meshes.Count + ci));
					SplitOptions adj = artificer.GetOptions(mindex + ci);
					if ( adj != null )
					{
						SplitOptions so1 = new SplitOptions();
						SplitOptions.Make(ref so1, so, adj);
						defso = so1;
						so = defso;
						//Debug.Log("sortmod " + so1.sortModifier.value + " index " + (mindex + ci));
					}
					else
					{
						Adjust adj1 = artificer.CheckInAdjust(me);	//.center);
						if ( adj1 )
						{
							SplitOptions so1 = new SplitOptions();
							SplitOptions.Make(ref so1, so, adj1.options);
							defso = so1;
							so = defso;
						}
						else
							defso = so = baseso;
					}
				}
				else
				{
					defso = so = baseso;
				}

				if ( defso.sortSpline.value && defso.useSortSpline.value )	//opts.spline )
				{
					SplineContainer spline = defso.sortSpline.value;
					float3 point = spline.transform.InverseTransformPoint(opts.root.TransformPoint(me.center));	//opts.tm.TransformPoint(me.center));
					float3 nearest;
					float alpha;
					float dist = SplineUtility.GetNearestPoint<Spline>(spline.Spline, point, out nearest, out alpha);

					point = opts.tm.InverseTransformPoint(spline.transform.TransformPoint(nearest));

					switch ( so.sortDistanceMode.value )
					{
						case SortDistanceMode.Closest:
							me.closestPoint = me.bounds.ClosestPoint(point);    //tm.InverseTransformPoint(point));
							break;

						case SortDistanceMode.Furthest:
							me.closestPoint = me.bounds.FurthestPoint(point);    //tm.InverseTransformPoint(point));
							break;

						case SortDistanceMode.Centre:
							me.closestPoint = me.bounds.center;    //tm.InverseTransformPoint(point));
							break;
					}

					//me.closestPoint = me.bounds.ClosestPoint(point);    //tm.InverseTransformPoint(point));
					me.distance = (dist + (alpha * defso.sortPathBias.value)) + defso.sortModifier.value;  //sortMod;
					//me.distance = alpha + defso.sortModifier.value;  //sortMod;
					//Debug.Log("dist " + me.distance);

					//Debug.DrawLine(spline.transform.TransformPoint(p1), spline.transform.TransformPoint(nearest), Color.black, 10.0f);
					//Debug.DrawLine(spline.transform.TransformPoint(p1), spline.transform.TransformPoint(p1) + Vector3.up, Color.red, 10.0f);
				}
				else
				{
					switch ( so.sortDistanceMode.value )
					{
						case SortDistanceMode.Closest:
							me.closestPoint = me.center + me.bounds.ClosestPoint(defso.sortOrigin.value);    //tm.InverseTransformPoint(point));
							break;

						case SortDistanceMode.Furthest:
							me.closestPoint = me.center + me.bounds.FurthestPoint(defso.sortOrigin.value);    //tm.InverseTransformPoint(point));
							break;

						case SortDistanceMode.Centre:
							me.closestPoint = me.center + me.bounds.center;    //tm.InverseTransformPoint(point));
							break;
					}

					//me.closestPoint = me.center + me.bounds.ClosestPoint(defso.sortOrigin.value);
					if ( defso.sortMode.value == SortMode.Random )
					{
						me.distance = artificer.RandomValue() + defso.sortModifier.value;
					}
					else
						me.distance = Vector3.Distance(me.closestPoint, defso.sortOrigin.value) + defso.sortModifier.value;
				}

				//me.buildStyle = bstyle;
				//me.splitParams = sp;
				so.Copy(me, artificer);

				Vector3 dir = (me.center - me.origin);	// - me.center);  //.normalized;
				if ( dir.sqrMagnitude.Equals(0.0f) )
					dir = Vector3.up;

				dir.x *= me.projection.x;
				dir.y *= me.projection.y;
				dir.z *= me.projection.z;
				me.dir = dir.normalized;	// * me.buildDist * (1.0f - a));
				//Debug.Log("Dir " + me.dir);

				//if ( defso.buildFrom.value != null )	//&& defso.buildFrom.value.Length > 0 && defso.buildFrom.value[0] )
				{
					//Debug.Log("build from " + defso.buildFrom.value);
					artificer.CalcSpline(me, defso, opts.tm);	//defso.buildFrom.value, defso.startTension.value, defso.endTension.value);
				}

				me.matSortID = artificer.GetMatSortID(usedMats);
				//me.mr = opts.tm.GetComponent<MeshRenderer>();
				//me.gameObject = opts.tm.name;	// TODO: dont need
				me.renderer = mr;
#if UNITY_6000_3_OR_NEWER
				me.id = mr.gameObject.GetEntityId();
#else
				me.id = mr.gameObject.GetInstanceID();
#endif
				me.id = mr.GetComponent<ObjectID>().id;

				//me.index = meshElements.Count;

				meshElements.Add(me);
			}

			// Dispose native arrays
			nVertexKeys.Dispose();
			nPositions.Dispose();
			nUV0.Dispose();
			nUV1.Dispose();
			nNormals.Dispose();
			nColors.Dispose();
			adjacency.Dispose();
			nTrianglesAll.Dispose();
			nSubmeshOffsets.Dispose();
			nSubmeshTriCounts.Dispose();

			return meshElements;
		}

#if false
		static void CalcSpline(MeshElement me, Artificer mod, Transform buildfrom, float tension1, float tension2)
		{
			me.path = new Spline();

			Vector3 startvec, endvec, startpoint, endpoint;

			Quaternion rot1 = buildfrom.rotation;
			Quaternion rot2 = mod.transform.rotation;

			// Have a bounds box or sphere
			Vector3 lscl = buildfrom.localScale;
			Vector3 fp = Vector3.zero;
			fp.x = RandomRange(-mod.buildFromBox.x * 0.5f, mod.buildFromBox.x * 0.5f);
			fp.y = RandomRange(-mod.buildFromBox.y * 0.5f, mod.buildFromBox.y * 0.5f);
			fp.z = RandomRange(-mod.buildFromBox.z * 0.5f, mod.buildFromBox.z * 0.5f);

			fp += mod.buildFromOffset;

			fp = buildfrom.TransformPoint(fp);

			startpoint = mod.transform.worldToLocalMatrix.MultiplyPoint3x4(fp);
			endpoint = me.tm.GetPosition();

			startvec = tension1 * (rot1 * Vector3.up);

			Vector3 dir = Vector3.up;
			switch ( mod.splineDirCalc )
			{
				case SplineDir.Origin:		dir = (mod.origin - endpoint); break;
				case SplineDir.SortOrigin:	dir = (mod.sortOrigin - endpoint); break;
			}

			dir = Vector3.Scale(dir, mod.splineEndProject).normalized;

			endvec = tension2 * (rot2 * dir);

			me.path.Add(new BezierKnot(startpoint, -startvec, startvec));
			me.path.Add(new BezierKnot(endpoint, -endvec, endvec));
		}
#endif
	}
}