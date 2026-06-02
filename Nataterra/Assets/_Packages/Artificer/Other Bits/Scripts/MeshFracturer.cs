#if false
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

namespace Artifice
{
	public class MeshFracturer : MonoBehaviour
	{
		[Header("Fracture Settings")]
		public int sliceCount = 8;
		public Material crossSectionMaterial;
		public bool addRigidbodies = true;
		public float explosionForce = 3f;

		public void Fracture()
		{
			List<GameObject> pieces = new List<GameObject>();
			pieces.Add(gameObject);

			for (int i = 0; i < sliceCount; i++)
			{
				List<GameObject> newPieces = new List<GameObject>();

				foreach (GameObject piece in pieces)
				{
					if (piece == null) continue;

					Bounds bounds = GetWorldBounds(piece);

					// Pick a random point INSIDE the mesh bounds
					Vector3 planePoint = new Vector3(
						Random.Range(bounds.min.x, bounds.max.x),
						Random.Range(bounds.min.y, bounds.max.y),
						Random.Range(bounds.min.z, bounds.max.z)
					);

					// Random but stable direction
					Vector3 planeNormal = Random.onUnitSphere.normalized;

					SlicedHull hull = piece.Slice(planePoint, planeNormal, crossSectionMaterial);

					if (hull != null)
					{
						GameObject upper = hull.CreateUpperHull(piece, crossSectionMaterial);
						GameObject lower = hull.CreateLowerHull(piece, crossSectionMaterial);

						SetupPiece(upper, piece);
						SetupPiece(lower, piece);

						newPieces.Add(upper);
						newPieces.Add(lower);
						Destroy(piece);
					}
					else
					{
						newPieces.Add(piece);
					}
				}

				pieces = newPieces;
			}

			if (addRigidbodies)
			{
				foreach (GameObject piece in pieces)
				{
					Rigidbody rb = piece.GetComponent<Rigidbody>();
					rb.AddExplosionForce(explosionForce * 50f, transform.position, 3f);
				}
			}
		}

		private Bounds GetWorldBounds(GameObject go)
		{
			MeshRenderer mr = go.GetComponent<MeshRenderer>();
			if (mr != null)
				return mr.bounds;

			MeshFilter mf = go.GetComponent<MeshFilter>();
			Bounds b = mf.mesh.bounds;
			b.center = go.transform.TransformPoint(b.center);
			b.extents = Vector3.Scale(b.extents, go.transform.lossyScale);
			return b;
		}

		private void SetupPiece(GameObject piece, GameObject original)
		{
			piece.transform.SetPositionAndRotation(
				original.transform.position,
				original.transform.rotation
			);
			piece.transform.localScale = original.transform.localScale;

			MeshCollider collider = piece.AddComponent<MeshCollider>();
			collider.convex = true;

			piece.layer = LayerMask.NameToLayer("fracture");

			if (addRigidbodies)
			{
				Rigidbody rb = piece.AddComponent<Rigidbody>();
				rb.mass = 1f;
			}
		}

		void Update()
		{
			if ( Input.GetKeyDown(KeyCode.F) )
			{
				Fracture();
			}
		}
	}
}
#endif