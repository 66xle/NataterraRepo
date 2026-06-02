// Comment the line out below if you are not using URP
#define ARTIURP

using UnityEngine;
#if ARTIURP
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#endif

namespace Artifice
{
	public class DOFControl : MonoBehaviour
	{
//#if ARTIURP
//		DepthOfField dof;
//		CameraOrbit cam;

//		void Start()
//		{
//			cam = GetComponent<CameraOrbit>();
//			Volume volume = GetComponentInChildren<Volume>();
//			if ( volume )
//			{
//				volume.profile.TryGet<DepthOfField>(out dof);
//				if ( dof )
//					dof.active = true;
//			}
//		}

//		void Update()
//		{
//			if ( dof && cam )
//			{
//				if ( cam.path && cam.pathSpeed != 0.0f )
//					dof.focusDistance.value = Vector3.Distance(cam.transform.position, cam.target.transform.position);
//				else
//					dof.focusDistance.value = cam.distance;

//				dof.aperture.value = cam.target.aperture;
//				dof.focalLength.value = cam.target.focalLength;
//			}
//		}
//#endif
	}
}