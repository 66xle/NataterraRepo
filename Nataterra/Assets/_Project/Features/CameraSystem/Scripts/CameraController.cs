using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CinemachineCamera viewCam;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10f;

    public Collider boundingCollider;


    Terrain terrain;


    void Start()
    {
        terrain = Terrain.activeTerrain;
    }

    void Update()
    {
        Vector3 forward = viewCam.transform.forward;
        Vector3 right = viewCam.transform.right;

        // Flatten movement to XZ plane
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector2 moveInput = InputManager.Instance.MoveInput;

        Vector3 move = (forward * moveInput.y + right * moveInput.x).normalized;

        Vector3 current = viewCam.transform.position;
        Vector3 nextPos = current + move * moveSpeed * Time.deltaTime;
        Vector3 closest = boundingCollider.ClosestPoint(nextPos);



        viewCam.transform.position = closest;

        //Vector3 forward = viewCam.transform.forward;
        //Vector3 right = viewCam.transform.right;

        //forward.y = 0f;
        //right.y = 0f;

        //forward.Normalize();
        //right.Normalize();

        //Vector3 input =
        //    forward * moveInput.y +
        //    right * moveInput.x;

        //if (input.sqrMagnitude > 1f)
        //    input.Normalize();

        //Vector3 velocity = input * moveSpeed;

        //Vector3 current = viewCam.transform.position;
        //Vector3 delta = velocity * Time.deltaTime;
        //Vector3 next = current + delta;

        //Bounds b = boundingCollider.bounds;

        //// If inside  normal movement
        //if (b.Contains(next))
        //{
        //    viewCam.transform.position = next;
        //    return;
        //}

        //// -------------------------
        //// TRUE SLIDING
        //// -------------------------

        //Vector3 closest = new Vector3(
        //    Mathf.Clamp(next.x, b.min.x, b.max.x),
        //    0f,
        //    Mathf.Clamp(next.z, b.min.z, b.max.z)
        //);

        //// penetration direction
        //Vector3 penetration = next - closest;

        //// determine wall normal (axis-aligned box)
        //Vector3 normal;

        //if (Mathf.Abs(penetration.x) > Mathf.Abs(penetration.z))
        //    normal = new Vector3(Mathf.Sign(penetration.x), 0f, 0f);
        //else
        //    normal = new Vector3(0f, 0f, Mathf.Sign(penetration.z));

        //// project velocity onto wall plane
        //Vector3 slideVelocity =
        //    Vector3.ProjectOnPlane(velocity, normal);

        //// restore constant speed
        //if (slideVelocity.sqrMagnitude > 0.0001f)
        //    slideVelocity = slideVelocity.normalized * moveSpeed;
        //else
        //    slideVelocity = Vector3.zero;

        //Vector3 final =
        //    viewCam.transform.position + slideVelocity * Time.deltaTime;

        //// final safety clamp
        //if (b.Contains(final))
        //    viewCam.transform.position = final;
    }
}
