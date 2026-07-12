using UnityEngine;

public class FalseIbisCameraMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 18f;
    [SerializeField] private float fastMoveMultiplier = 2f;
    [SerializeField] private float movementSmoothing = 10f;

    [Header("Edge Scroll")]
    [SerializeField] private bool useEdgeScroll = true;
    [SerializeField] private float edgeSize = 16f;

    [Header("Global Orbit")]
    [SerializeField] private bool disableMovementInGlobalOrbit = true;

    private Vector3 desiredPivotPosition;
    private bool initialized;

    public void Initialize(Transform rig)
    {
        desiredPivotPosition = rig.position;
        initialized = true;
    }

    public void SnapDesiredPositionToRig(Transform rig)
    {
        desiredPivotPosition = rig.position;
    }

    public void Tick(
        Transform rig,
        FalseIbisCameraMode mode,
        float deltaTime)
    {
        if (!initialized)
            Initialize(rig);

        if (mode == FalseIbisCameraMode.GlobalOrbit && disableMovementInGlobalOrbit)
        {
            desiredPivotPosition = rig.position;
            return;
        }

        Vector3 input = GetKeyboardInput();

        if (useEdgeScroll && mode == FalseIbisCameraMode.Strategy)
            input += GetEdgeInput();

        if (input.sqrMagnitude > 1f)
            input.Normalize();

        float speed = Input.GetKey(KeyCode.LeftShift)
            ? moveSpeed * fastMoveMultiplier
            : moveSpeed;

        Vector3 forward = rig.forward;
        Vector3 right = rig.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 worldMove =
            forward * input.z +
            right * input.x;

        desiredPivotPosition += worldMove * speed * deltaTime;

        rig.position = Vector3.Lerp(
            rig.position,
            desiredPivotPosition,
            1f - Mathf.Exp(-movementSmoothing * deltaTime)
        );
    }

    private Vector3 GetKeyboardInput()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            input += Vector3.forward;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            input += Vector3.back;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            input += Vector3.left;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            input += Vector3.right;

        return input;
    }

    private Vector3 GetEdgeInput()
    {
        Vector3 input = Vector3.zero;
        Vector3 mouse = Input.mousePosition;

        if (mouse.x <= edgeSize)
            input += Vector3.left;
        else if (mouse.x >= Screen.width - edgeSize)
            input += Vector3.right;

        if (mouse.y <= edgeSize)
            input += Vector3.back;
        else if (mouse.y >= Screen.height - edgeSize)
            input += Vector3.forward;

        return input;
    }
}