using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace Artifice
{
	[System.Serializable]
	public class MeshTris
	{
		public int[]	tris;
	}

	[System.Serializable]
	public class MeshElement
	{
		public Mesh					mesh;
		public Vector3				position;
		public Quaternion			rotation;
		public Vector3				scale;
		public Vector3				center;
		public Vector3				axisPoint;
		public Bounds				bounds;
		public Material[]			mats;
		public Matrix4x4			tm;
		public Matrix4x4			tm1;
		public Vector3				closestPoint;
		public float				distance;
		public BuildStyle			buildStyle;
		public DismantleStyle		dismantleStyle;
		public RenderParams[]		rp;
		[SerializeReference]
		public List<MeshElement>	children;
		[SerializeReference]
		public string				gameObject;
		public int					subMeshCount;
		public Vector3[]			verts;
		public Vector3[]			normals;
		public Vector4[]			tangents;
		public Vector2[]			uvs;
		public Vector2[]			uv2;
		public Color[]				colors;
		public MeshTris[]			tris;
		public bool					havePath;
		public Spline				path;
		public int					matSortID;
		public List<int>			draw;
		public float				buildDist;
		public float				buildTime;
		public float				dismantleTime;
		public bool					usePlaceCurve;
		public AnimationCurve		placeCurve;
		public bool					useRotCurve;
		public AnimationCurve		placeRotCurve;
		public bool					useScaleCurve;
		public bool					perAxisScale;
		public AnimationCurve		placeScaleCurve;
		public AnimationCurve		placeScaleCurveY;
		public AnimationCurve		placeScaleCurveZ;
		public float				maxScale;
		public Vector3				rotate;
		public Vector3				origin;
		public Vector3				sortOrigin;
		public Vector3				projection;
		public float				placeTime;
		public PlaceMode			placeMode;
		public LastElement			lastElement;
		public bool					firstElement;
		public Renderer				renderer;   // only used during build
		public int					id;
		public float				bt;
		public Vector3				dir;
		public float				removeTime;
		public float				minExplodeForce;
		public float				maxExplodeForce;
		public Vector3				dismantleRotate;
		public CollisionMode		collisionMode;
		public LayerMask			layers;
		public float				collisionY;
		public Vector3				angVelRange;
		public float				gravityModifier;
		public Vector3				dismantleProjection;
		public float				bounce;
		public float				linearDrag;
		public float				angularDrag;
	}
}