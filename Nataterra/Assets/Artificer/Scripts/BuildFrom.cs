using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

namespace Artifice
{
	[System.Serializable]
	public class BuildFrom
	{
		public Transform	buildFrom;
		public Vector3		buildFromBox			= Vector3.zero;
		public Vector3		buildFromOffset			= Vector3.zero;
		public Vector3		startDir				= Vector3.up;
		public float		startTension			= 1.0f;
		public float		endTension				= 1.0f;
		public SplineDir	splineDirCalc			= SplineDir.SortOrigin;
		public Vector3		splineEndProject		= new Vector3(0.0f, 1.0f, 0.0f);
		public SplineMode	splineMode				= SplineMode.BuildFrom;
		[HideInInspector]
		public bool			initialized;

		public void Init()
		{
			if ( !initialized )
			{
				buildFromBox		= Vector3.zero;
				buildFromOffset		= Vector3.zero;
				startTension		= 1.0f;
				endTension			= 1.0f;
				splineDirCalc		= SplineDir.SortOrigin;
				splineEndProject	= new Vector3(0.0f, 1.0f, 0.0f);
				splineMode			= SplineMode.BuildFrom;
				initialized			= true;
			}
		}

		float GetNearestPoint(SplineContainer spline, Vector3 pos)
		{
			Vector3 lpos = spline.transform.InverseTransformPoint(pos);
			float3 nearf3;
			float alpha;

			return SplineUtility.GetNearestPoint(spline.Spline, lpos, out nearf3, out alpha, 4, 2);
		}

		public float GetDist(Vector3 pos)
		{
			if ( buildFrom )
			{
				SplineContainer spl = buildFrom.GetComponent<SplineContainer>();

				if ( spl )
					return GetNearestPoint(spl, pos);
				else
					return Vector3.Distance(buildFrom.position, pos);
			}

			return float.MaxValue;
		}
	}
}