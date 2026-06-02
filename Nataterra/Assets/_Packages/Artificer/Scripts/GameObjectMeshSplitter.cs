using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines;
using Unity.Mathematics;

namespace Artifice
{
	[System.Flags]
	public enum SplitModes
	{
		Element		= 1,
		Material	= 2,
		UV1			= 4,
		UV2			= 8,
		Normal		= 16,
		Color		= 32,
	}

	public static class GameObjectMeshSplitter
	{
		//static Dictionary<EntityId, MeshElement>	meshSet;

		public static List<MeshElement> SplitGameObject(Artificer artificer, GameObject go, Vector3 sortOrigin, SplineContainer spline, bool useParams, float weldTolerance = 0.0001f)
		{
			//if ( meshSet == null )
			//{
			//	meshSet = new Dictionary<EntityId, MeshElement>();
			//}
			//meshSet.Clear();

			Animator anim = go.GetComponent<Animator>();

			if ( anim )
			{
#if UNITY_EDITOR
				UnityEditor.AnimationMode.StartAnimationMode();
				AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
				//Debug.Log("Clipinfo " + clipinfo + " len " + clipinfo.Length);
				if ( clipinfo != null && clipinfo.Length > 0 && clipinfo[0].clip )
					UnityEditor.AnimationMode.SampleAnimationClip(go, clipinfo[0].clip, 0.0f);
#endif
			}

			List<MeshElement> meshes = new List<MeshElement>();

			//ProcessRecursive(artificer, go, go.transform, ref meshes, sortOrigin, 0.0f, spline, useParams, weldTolerance);
			ProcessRecursive(artificer, go, artificer.transform, ref meshes, sortOrigin, 0.0f, spline, useParams, weldTolerance);

			if ( anim )
			{
#if UNITY_EDITOR
				UnityEditor.AnimationMode.StopAnimationMode();
#endif
			}

			return meshes;
		}

		static int SortElementsVerticalBounds(MeshElement m1, MeshElement m2)
		{
			if ( m1.distance > m2.distance ) return 1;
			if ( m1.distance < m2.distance ) return -1;

			return 0;
		}


		private static void ProcessRecursive(Artificer artificer, GameObject go, Transform root, ref List<MeshElement> meshes, Vector3 sortOrigin, float sortModifier, SplineContainer spline, bool useParams, float weldTolerance)
		{
			Debug.Log("go " + go.name + " root " + root.name);
			//Debug.Log("go " + go.name + " enabled " + go.activeSelf);
			if ( !go.activeSelf )
				return;

			if ( artificer.exclude.Contains(go) )
				return;

			var mf = go.GetComponent<MeshFilter>();
			//var mr = go.GetComponent<MeshRenderer>();
			var mr = go.GetComponent<Renderer>();

			Mesh sharedMesh = null;

			if ( mf )
				sharedMesh = mf.sharedMesh;

			//if ( sharedMesh )
			//	Debug.Log("mesh id " + sharedMesh.GetEntityId() + " mn " + sharedMesh.name + " goid " + go.GetEntityId());

			if ( mr.GetType() == typeof(SkinnedMeshRenderer) )
			{
				SkinnedMeshRenderer _sr = (SkinnedMeshRenderer)mr;
				sharedMesh = new Mesh();
				_sr.BakeMesh(sharedMesh);
				//sharedMesh.RecalculateBounds();
				//sharedMesh.RecalculateNormals();
				//sharedMesh.RecalculateTangents();
				//sharedMesh = _sr.sharedMesh;
			}

			//if ( mf != null && mr != null && mf.sharedMesh != null )
			if ( mr != null && sharedMesh != null )
			{
#if false
				MeshElement useme = null;

				if ( meshSet.ContainsKey(sharedMesh.GetEntityId()) )
				{
					useme = meshSet[sharedMesh.GetEntityId()];
					Debug.Log("Add this mesh already id " + sharedMesh.GetEntityId() + " mn " + sharedMesh.name);
				}
				else
					meshSet[sharedMesh.GetEntityId()] = 
#endif
				SplitOptions so = new SplitOptions();

				so.SetDefault(artificer);

				if ( useParams )
				{
					SplitParams sp = go.GetComponent<SplitParams>();
					if ( !sp || (sp && !sp.enabled) )
					{
						if ( go.transform.parent )
						{
							SplitParams psp = go.transform.parent.GetComponentInParent<SplitParams>();
							if ( psp && psp.enabled && psp.applyToChildren )
								sp = psp;
						}
					}

					if ( sp && sp.enabled )
					{
						SplitOptions so1 = new SplitOptions();
						SplitOptions.Make(ref so1, so, sp.splitOptions);
						
						so = so1;
					}
				}

				if ( !so.dontAdd.value )
				{
					if ( so.dontSplit.value )
					{
						MeshElement me = new MeshElement();

						Mesh smesh = sharedMesh;	//mf.sharedMesh;

						me.subMeshCount	= smesh.subMeshCount;
						me.verts		= smesh.vertices;
						me.normals		= smesh.normals;
						me.tangents		= smesh.tangents;
						me.colors		= smesh.colors;
						me.uvs			= smesh.uv;
						me.uv2			= smesh.uv2;
						me.tris			= new MeshTris[smesh.subMeshCount];
						me.draw			= new List<int>();

						//Matrix4x4 meshtm = go.transform.localToWorldMatrix * root.worldToLocalMatrix;
						Matrix4x4 meshtm = root.worldToLocalMatrix * go.transform.localToWorldMatrix;

						// We need to get normals, tangents and verts into the local space of the root
						for ( int i = 0; i < me.verts.Length; i++ )
						{
							me.verts[i]		= meshtm.MultiplyPoint(me.verts[i]);
							me.normals[i]	= meshtm.MultiplyVector(me.normals[i]);
							me.tangents[i]	= meshtm.MultiplyVector(me.tangents[i]);
						}


						for ( int s = 0; s < smesh.subMeshCount; s++ )
						{
							int[] tris = smesh.GetTriangles(s);

							if ( tris.Length > 0 )
							{
								me.draw.Add(s);
								me.tris[s] = new MeshTris();
								me.tris[s].tris = tris;
							}
							else
								me.tris[s].tris = System.Array.Empty<int>();
						}

						me.position		= go.transform.localPosition;
						me.rotation		= go.transform.localRotation;
						me.scale		= go.transform.localScale;
						me.center		= smesh.bounds.center;
						me.mats			= mr.sharedMaterials;
						me.tm			= MeshSplitterHybrid.WorldToLocalMatrix(go.transform.localToWorldMatrix, root);
						me.axisPoint	= new Vector3(0.0f, me.center.y, 0.0f);
						me.bounds		= MeshSplitterHybrid.TransformBounds(smesh.bounds, go.transform.localToWorldMatrix);

						me.tm1			= root.localToWorldMatrix;	// * pivotTM;	//Matrix4x4.identity;	//pivotTM;	//opts.tm.localToWorldMatrix * pivotTM;
						me.tm			= MeshSplitterHybrid.WorldToLocalMatrix(me.tm1, root);	// opts.root.localToWorldMatrix;	//.worldToLocalMatrix;	//WorldToLocalMatrix(me.tm1, opts.root);

						if ( spline )
						{
							//float3 point = go.transform.TransformPoint(me.center);
							float3 point = root.transform.TransformPoint(me.center);
							float3 nearest;
							float alpha;
							float dist = SplineUtility.GetNearestPoint<Spline>(spline.Spline, point, out nearest, out alpha);

							//me.closestPoint = me.bounds.FurthestPoint(go.transform.InverseTransformPoint(point));

							me.closestPoint = me.bounds.ClosestPoint(go.transform.InverseTransformPoint(point));
							me.distance = (dist * alpha) + so.sortModifier.value;
						}
						else
						{
							//me.closestPoint = me.bounds.FurthestPoint(sortOrigin);

							me.closestPoint = me.bounds.ClosestPoint(sortOrigin);
							me.distance		= Vector3.Distance(me.closestPoint, sortOrigin) + so.sortModifier.value;
						}

						//me.gameObject	= go.name;	// May not need
						me.renderer		= mr;
						me.lastElement = LastElement.Last;
						if ( me.renderer && me.renderer.GetComponentInParent<LODGroup>() )
							me.lastElement |= LastElement.LOD;

#if UNITY_6000_3_OR_NEWER
						me.id = mr.gameObject.GetEntityId();
#else
						me.id = mr.gameObject.GetInstanceID();
#endif
						me.id = mr.GetComponent<ObjectID>().id;
						so.Copy(me, artificer);

						artificer.CalcSpline(me, so, go.transform);

						meshes.Add(me);
					}
					else
					{
						Mesh sourceMesh = sharedMesh;	//mf.sharedMesh;

						//Matrix4x4 meshtm = go.transform.localToWorldMatrix * root.worldToLocalMatrix;
						Matrix4x4 meshtm = root.worldToLocalMatrix * go.transform.localToWorldMatrix;	// * root.worldToLocalMatrix;

						Material[] sourceMaterials = mr.sharedMaterials;

						artificer.RemapMaterials(ref sourceMaterials);

						MeshSplitterHybrid.SplitValues so1 = new MeshSplitterHybrid.SplitValues();

						so1.root	= root;
						so1.tm		= go.transform;
						so1.mats	= sourceMaterials;

						List<MeshElement> splitMeshes = MeshSplitterHybrid.SplitMesh(artificer, meshtm, sourceMesh, so1, so, mr, meshes.Count);

						if ( so.addAll.value && splitMeshes != null )
						{
							splitMeshes.Sort(SortElementsVerticalBounds);
							for ( int i = 1; i < splitMeshes.Count; i++ )
								splitMeshes[i].distance = splitMeshes[0].distance;

							meshes.AddRange(splitMeshes);
						}
						else
						{
							if ( splitMeshes != null )
								meshes.AddRange(splitMeshes);
						}
					}
				}
			}

			LODGroup lg = go.GetComponent<LODGroup>();

			if ( lg && go.transform.childCount > 0)
			{
				LOD[] lods = lg.GetLODs();
				if ( lods.Length > 0 && lods[0].renderers.Length > 0 )
				{
					for ( int j = 0; j < lods[0].renderers.Length; j++ )
					{
						ProcessRecursive(artificer, lods[0].renderers[j].gameObject, root, ref meshes, sortOrigin, sortModifier, spline, useParams, weldTolerance);
					}
				}
			}
			else
			{
				for ( int i = 0; i < go.transform.childCount; i++ )
					ProcessRecursive(artificer, go.transform.GetChild(i).gameObject, root, ref meshes, sortOrigin, sortModifier, spline, useParams, weldTolerance);
			}
		}
	}
}