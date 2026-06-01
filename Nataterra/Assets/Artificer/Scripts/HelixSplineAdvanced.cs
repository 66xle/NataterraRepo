using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Artifice
{
	[ExecuteAlways]
	[RequireComponent(typeof(SplineContainer))]
	[AddComponentMenu("Splines/Helix Spline Advanced")]
	public class HelixSplineAdvanced : MonoBehaviour
	{
		public enum UpVectorMode { Axis, Automatic, Mixed }

		[Header("Helix Shape")]
		public float			radius1					= 1f;
		public float			radius2					= 0.75f;
		public float			height					= 0f;
		public float			turns					= 1f;
		[Range(-1f, 1f)]
		public float			bias					= 0f;
		public bool				clockwise				= true;
		[Range(3, 40)]
		public int				pointsPerTurn			= 4;
		public bool				loop					= false;

		[Header("Twisting / Orientation")]
		public UpVectorMode		upMode					= UpVectorMode.Mixed;
		public Vector3			axis					= Vector3.up;
		public bool				useTurnBasedTwist		= true;
		public float			extraTwistDegrees		= 0f;

		[Header("Loop Options")]
		public bool				loopBlendTangents		= true;
		public bool				loopForceMatch			= false;

		[Header("Generation")]
		public float			vectorLengthEstimate	= 0.5517861843f;
		public bool				generateOnValidate		= true;
		public bool				reverse					= false;

		private SplineContainer container;

		private void OnEnable()
		{
			container = GetComponent<SplineContainer>();
			if ( generateOnValidate ) Regenerate();
		}

		private void OnValidate()
		{
			pointsPerTurn = Mathf.Clamp(pointsPerTurn, 3, 500);
			if ( generateOnValidate ) Regenerate();
		}

		public void Regenerate()
		{
			if ( container == null ) container = GetComponent<SplineContainer>();
			if ( container == null ) return;

			// Clear existing splines
			var existing = new System.Collections.Generic.List<Spline>(container.Splines);
			foreach ( var s in existing )
				container.RemoveSpline(s);

			// Add new spline
			Spline spline = container.AddSpline();

			int pointCount = Mathf.Max(1, Mathf.RoundToInt(turns * pointsPerTurn));
			float deltaRadius = radius2 - radius1;
			float invPoints = 1f / pointCount;
			Vector3 axisNorm = axis == Vector3.zero ? Vector3.up : axis.normalized;
			float totalAngle = 2f * Mathf.PI * turns * (clockwise ? -1f : 1f);

			// Precompute positions and segment lengths
			Vector3[] positions = new Vector3[pointCount + 1];
			float[] segmentLengths = new float[pointCount];
			for ( int i = 0; i <= pointCount; i++ )
			{
				float t = i * invPoints;
				float r = radius1 + deltaRadius * t;

				// Apply bias to height
				float hpct = 0f;
				if ( bias > 0f ) hpct = 1f - Mathf.Pow(1f - t, 9f * bias + 1f);
				else if ( bias < 0f ) hpct = Mathf.Pow(t, 9f * -bias + 1f);
				else hpct = t;
				float z = height * hpct;

				float angle = totalAngle * t;

				Vector3 planeRight = Vector3.Cross(Vector3.up, axisNorm).sqrMagnitude > 0.001f
					? Vector3.Cross(Vector3.up, axisNorm).normalized
					: Vector3.right;
				Vector3 planeUp = Vector3.Cross(axisNorm, planeRight);

				positions[i] = planeRight * (Mathf.Cos(angle) * r) + planeUp * (Mathf.Sin(angle) * r) + axisNorm * z;

				if ( i > 0 )
					segmentLengths[i - 1] = (positions[i] - positions[i - 1]).magnitude;
			}

			// Add knots with proper tangents
			for ( int i = 0; i <= pointCount; i++ )
			{
				float t = i * invPoints;
				float r = radius1 + deltaRadius * t;
				float dr = deltaRadius;
				float theta = totalAngle * t;
				float dtheta = totalAngle;

				// Correct derivative of bias-adjusted height
				float dh = 1f;
				if ( bias > 0f )
				{
					float p = 9f * bias + 1f;
					dh = p * Mathf.Pow(1f - t, p - 1f);
				}
				else if ( bias < 0f )
				{
					float p = 9f * -bias + 1f;
					dh = p * Mathf.Pow(t, p - 1f);
				}
				float dz = height * dh;

				Vector3 planeRight = Vector3.Cross(Vector3.up, axisNorm).sqrMagnitude > 0.001f
					? Vector3.Cross(Vector3.up, axisNorm).normalized
					: Vector3.right;
				Vector3 planeUp = Vector3.Cross(axisNorm, planeRight);

				Vector3 deriv = planeRight * (-r * Mathf.Sin(theta) * dtheta + dr * Mathf.Cos(theta))
							  + planeUp * (r * Mathf.Cos(theta) * dtheta + dr * Mathf.Sin(theta))
							  + axisNorm * dz;

				// Scale derivative to match segment length
				float len = 0f;
				if ( i == 0 ) len = segmentLengths[0] / 3f;
				else if ( i == pointCount ) len = segmentLengths[pointCount - 1] / 3f;
				else len = ((segmentLengths[i - 1] + segmentLengths[i]) / 2f) / 3f;

				Vector3 tangent = deriv.normalized * len;

				// Local frame for twisting
				Vector3 T = tangent.normalized;
				Vector3 N = Vector3.Cross(axisNorm, T).normalized;
				Vector3 B = Vector3.Cross(T, N);

				// Twist applied around tangent
				float twistDeg = extraTwistDegrees * t;
				if ( useTurnBasedTwist )
				{
					twistDeg += turns * 360f * t;
					if ( clockwise ) twistDeg *= -1f;
				}
				Quaternion twistRot = Quaternion.AngleAxis(twistDeg, T);

				Vector3 tangentOut = twistRot * tangent;
				Vector3 tangentIn = twistRot * -tangent;

				spline.Add(new BezierKnot(positions[i], tangentIn, tangentOut));
			}

			spline.Closed = loop;

			if ( reverse )
			{
				container.ReverseFlow(0);
			}

	#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(container);
	#endif
		}


		public void Reverse()
		{
			container.ReverseFlow(0);
		}

		public void CenterPivot()
		{
			if ( container == null ) container = GetComponent<SplineContainer>();
			var spline = container.Spline;
			if ( spline == null || spline.Count == 0 ) return;

			Vector3 min = spline[0].Position;
			Vector3 max = spline[0].Position;
			for ( int i = 0; i < spline.Count; i++ )
			{
				min = Vector3.Min(min, spline[i].Position);
				max = Vector3.Max(max, spline[i].Position);
			}
			Vector3 center = (min + max) * 0.5f;

			for ( int i = 0; i < spline.Count; i++ )
			{
				var k = spline[i];
				spline[i] = new BezierKnot(k.Position - (float3)center, k.TangentIn, k.TangentOut);
			}

	#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(container);
	#endif
		}
	}

}