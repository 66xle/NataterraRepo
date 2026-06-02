using UnityEngine;
using UnityEngine.Splines;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Artifice
{
	[ExecuteInEditMode]
	public class CameraOrbit : MonoBehaviour
	{
		public Target	target;
		public float	distance	= 10.0f;
		public float	xSpeed		= 250.0f;
		public float	ySpeed		= 120.0f;
		public float	zSpeed		= 120.0f;
		public float	yMinLimit	= -20.0f;
		public float	yMaxLimit	= 80.0f;
		public float	x			= 0.0f;
		public float	y			= 0.0f;
		public Vector3	offset;
		public float	delay		= 0.2f;
		public float	delayz		= 0.2f;
		public float	mindist		= 1.0f;
		public float	minY		= 0.0f;
		public float	shakeAmt	= 1.0f;
		float			vx			= 0.0f;
		float			vy			= 0.0f;
		float			vz			= 0.0f;
		Vector3			center;
		Vector3			tpos;
		CameraShake		shake;
		Target[]		targets;
		int				tindex;
		// this in target, list of paths
		public float	pathAlpha	= 0.0f;
		public float	pathSpeed	= 0.0f;
		public SplineContainer	path;

		void Start()
		{
			shake = GetComponent<CameraShake>();
#if UNITY_2023_1_OR_NEWER
			targets = FindObjectsByType<Target>(FindObjectsSortMode.None);
#else
			targets = FindObjectsOfType<Target>();
#endif

			for ( int i = 0; i < targets.Length; i++ )
			{
				if ( targets[i] == target )
				{
					tindex = i;
					break;
				}
			}

			SetTarget(tindex);
		}

		void SetTarget(int tindex)
		{
			target = targets[tindex];

			x = target.camX;
			y = target.camY;
			distance = target.camZ;
		}


		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
		{
			smoothTime = Mathf.Max(0.0001F, smoothTime);
			float omega = 2f / smoothTime;
			float x = omega * deltaTime;
			float exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);
			float change = current - target;
			float maxChange = maxSpeed * smoothTime;
			change = Mathf.Clamp(change, -maxChange, maxChange);
			float temp = (currentVelocity + omega * change) * deltaTime;
			currentVelocity = (currentVelocity - omega * temp) * exp;
			float output = current - change + (change + temp) * exp;

			if ( (target - current) * (output - target) > 0f )
			{
				output = target;
				currentVelocity = (output - target) / deltaTime;
			}
			return output;
		}

		void Update()
		{
			if ( !Application.isPlaying )
			{
				CamUpdate();
			}
			else
			{
#if ENABLE_LEGACY_INPUT_MANAGER
				if ( Input.GetKeyDown(KeyCode.C) )
				{
					ScreenCapture.CaptureScreenshot("Artificer " + System.DateTime.Now.ToString("yyyy-dd-M-HH-mm_ss") + ".png", 1);
				}
#endif
				CamUpdate();

#if ENABLE_LEGACY_INPUT_MANAGER
				if ( Input.GetKeyDown(KeyCode.RightArrow) )
				{
					tindex++;
					if ( tindex >= targets.Length )
						tindex = 0;

					SetTarget(tindex);
				}

				if ( Input.GetKeyDown(KeyCode.LeftArrow) )
				{
					tindex--;
					if ( tindex < 0 )
						tindex = targets.Length - 1;

					SetTarget(tindex);
				}
#else
				if ( Keyboard.current.leftArrowKey.isPressed )
				{
					tindex++;
					if ( tindex >= targets.Length )
						tindex = 0;

					SetTarget(tindex);
				}

				if ( Keyboard.current.rightArrowKey.isPressed )
				{
					tindex--;
					if ( tindex < 0 )
						tindex = targets.Length - 1;

					SetTarget(tindex);
				}
#endif
				targets[tindex].DoUpdate();
			}
		}

		Vector3 cspeed;
		Vector3 vel;

		public float	moveSpeed	= 1.0f;
		public float	moveDelay	= 0.5f;

		void CamUpdate()
		{
			if ( target )
			{
				Vector3 shakePos = Vector3.zero;
				Vector3 shakeRot = Vector3.zero;

				if ( shake )
					shake.DoUpdate(ref shakePos, ref shakeRot, shakeAmt);

				if ( path && pathSpeed != 0.0f )
				{
#if ENABLE_LEGACY_INPUT_MANAGER
					if ( Input.GetMouseButton(1) )
					{
						pathAlpha += Input.GetAxis("Mouse X") * 0.001f;
					}
					else
						pathAlpha += pathSpeed * Time.deltaTime;
#else
					if ( Mouse.current.rightButton.isPressed )
					{
						pathAlpha += Mouse.current.delta.x.ReadValue();	// Input.GetAxis("Mouse X") * 0.001f;
					}
					else
						pathAlpha += pathSpeed * Time.deltaTime;
#endif
					while ( pathAlpha > 1.0f )
						pathAlpha -= 1.0f;

					while ( pathAlpha < 0.0f )
						pathAlpha += 1.0f;

					//pathAlpha %= 1.0f;
					transform.position = (Vector3)path.EvaluatePosition(pathAlpha) + shakePos;	// SplineUtility.EvaluatePosition<SplineContainer>()
					Quaternion look = Quaternion.LookRotation(target.transform.position - transform.position);
					transform.rotation = look * Quaternion.Euler(shakeRot);	//.LookAt(target.transform.position);
				}
				else
				{
					if ( Application.isPlaying )
					{
#if ENABLE_LEGACY_INPUT_MANAGER
						if ( Input.GetMouseButton(1) )
						{
							target.camX = x + Input.GetAxis("Mouse X") * xSpeed;
							target.camY = y - Input.GetAxis("Mouse Y") * ySpeed;
							target.camZ = distance - (Input.GetAxis("Mouse ScrollWheel") * zSpeed);
						}
						else
							target.camX += target.spinSpeed * Time.deltaTime;
#else
						if ( Mouse.current.rightButton.isPressed )
						{
							target.camX = x + Mouse.current.delta.x.ReadValue() * xSpeed * 0.2f;
							target.camY = y - Mouse.current.delta.y.ReadValue() * ySpeed * 0.2f;
							target.camZ = distance - (Mouse.current.scroll.ReadValue().y * zSpeed * 0.2f);
						}
						else
							target.camX += target.spinSpeed * Time.deltaTime;
#endif
					}

					if ( Application.isPlaying )
					{
						x = SmoothDamp(x, target.camX, ref vx, delay, float.MaxValue, Time.deltaTime);
						y = SmoothDamp(y, target.camY, ref vy, delay, float.MaxValue, Time.deltaTime);
						distance = SmoothDamp(distance, target.camZ, ref vz, delayz, float.MaxValue, Time.deltaTime);
					}
					else
					{
						x = target.camX;
						y = target.camY;
						distance = target.camZ;
					}

					y = ClampAngle(y, yMinLimit, yMaxLimit);

					if ( distance < mindist )
					{
						distance = mindist;
						target.camZ = mindist;
					}

					Vector3 speed = Vector3.zero;
#if ENABLE_LEGACY_INPUT_MANAGER
					if ( Input.GetKey(KeyCode.W) )
						speed.z = moveSpeed;

					if ( Input.GetKey(KeyCode.S) )
						speed.z = -moveSpeed;

					if ( Input.GetKey(KeyCode.A) )
						speed.x = -moveSpeed;

					if ( Input.GetKey(KeyCode.D) )
						speed.x = moveSpeed;

					if ( Input.GetKey(KeyCode.E) )
						speed.y = moveSpeed;

					if ( Input.GetKey(KeyCode.Q) )
						speed.y = -moveSpeed;
#else
					if ( Keyboard.current.wKey.isPressed )
						speed.z = moveSpeed;

					if ( Keyboard.current.sKey.isPressed )
						speed.z = -moveSpeed;

					if ( Keyboard.current.aKey.isPressed )
						speed.x = -moveSpeed;

					if ( Keyboard.current.dKey.isPressed )
						speed.x = moveSpeed;

					if ( Keyboard.current.eKey.isPressed )
						speed.y = moveSpeed;

					if ( Keyboard.current.qKey.isPressed )
						speed.y = -moveSpeed;
#endif
					cspeed = Vector3.SmoothDamp(cspeed, speed, ref vel, moveDelay);

					Vector3 p = target.transform.position;

					Vector3 fwd = transform.forward;
					fwd.y = 0.0f;

					p += fwd.normalized * cspeed.z;
					p += transform.right * cspeed.x;
					p += Vector3.up * cspeed.y;

					target.transform.position = p;

					Vector3 c = target.transform.TransformPoint(center + offset);

					tpos = c;

					Quaternion rotation = Quaternion.Euler(y, x, 0.0f);

					Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + tpos;	//c;

					transform.rotation = rotation * Quaternion.Euler(shakeRot);

					if ( position.y < minY )
					{
						position.y = minY;
						cspeed.y = 0.0f;
					}

					transform.position = position + shakePos;
				}
			}
		}

		static float ClampAngle(float angle, float min, float max)
		{
			while ( angle < -360.0f )
				angle += 360.0f;

			while ( angle > 360.0f )
				angle -= 360.0f;

			return Mathf.Clamp(angle, min, max);
		}
	}
}