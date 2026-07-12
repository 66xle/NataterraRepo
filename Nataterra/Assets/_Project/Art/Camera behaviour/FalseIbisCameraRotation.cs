using UnityEngine;

public class FalseIbisCameraRotation : MonoBehaviour
{
    [Header("Keyboard Rotation")]
    [SerializeField] private float keyboardRotationSpeed = 90f;

    [Header("Mouse Rotation")]
    [SerializeField] private bool useMiddleMouseRotation = true;
    [SerializeField] private float mouseRotationSpeed = 3f;

    [Header("Smoothing")]
    [SerializeField] private float rotationSmoothing = 14f;

    [Header("Global Orbit")]
    [SerializeField] private float globalOrbitKeyboardYawSpeed = 65f;
    [SerializeField] private float globalOrbitMouseYawSpeed = 3f;
    [SerializeField] private float globalOrbitMousePitchSpeed = 1.5f;
    [SerializeField] private float globalOrbitPitchSmoothing = 10f;
    [SerializeField] private float defaultGlobalOrbitPitch = 45f;
    [SerializeField] private float minGlobalOrbitPitch = 18f;
    [SerializeField] private float maxGlobalOrbitPitch = 78f;

    public bool HadInputThisFrame { get; private set; }
    public float GlobalOrbitPitch { get; private set; }
    public float GlobalOrbitYaw => currentYaw;

    private float targetYaw;
    private float currentYaw;
    private float targetGlobalOrbitPitch;
    private bool initialized;

    public void Initialize(Transform rig)
    {
        targetYaw = rig.eulerAngles.y;
        currentYaw = targetYaw;

        GlobalOrbitPitch = defaultGlobalOrbitPitch;
        targetGlobalOrbitPitch = defaultGlobalOrbitPitch;

        initialized = true;
    }

    public void Tick(Transform rig, FalseIbisCameraMode mode, float deltaTime)
    {
        if (!initialized)
            Initialize(rig);

        HadInputThisFrame = false;

        if (mode == FalseIbisCameraMode.GlobalOrbit)
            TickGlobalOrbitRotation(rig, deltaTime);
        else
            TickStandardRotation(rig, deltaTime);
    }

    private void TickStandardRotation(Transform rig, float deltaTime)
    {
        float yawDelta = 0f;

        if (Input.GetKey(KeyCode.Q)) yawDelta -= keyboardRotationSpeed * deltaTime;
        if (Input.GetKey(KeyCode.E)) yawDelta += keyboardRotationSpeed * deltaTime;

        if (useMiddleMouseRotation && Input.GetMouseButton(2))
            yawDelta += Input.GetAxis("Mouse X") * mouseRotationSpeed;

        if (Mathf.Abs(yawDelta) > 0.001f)
            HadInputThisFrame = true;

        targetYaw += yawDelta;

        currentYaw = Mathf.LerpAngle(
            currentYaw,
            targetYaw,
            1f - Mathf.Exp(-rotationSmoothing * deltaTime)
        );

        rig.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

    private void TickGlobalOrbitRotation(Transform rig, float deltaTime)
    {
        float yawDelta = 0f;

        if (Input.GetKey(KeyCode.Q)) yawDelta -= globalOrbitKeyboardYawSpeed * deltaTime;
        if (Input.GetKey(KeyCode.E)) yawDelta += globalOrbitKeyboardYawSpeed * deltaTime;

        if (useMiddleMouseRotation && Input.GetMouseButton(2))
        {
            yawDelta += Input.GetAxis("Mouse X") * globalOrbitMouseYawSpeed;

            targetGlobalOrbitPitch -= Input.GetAxis("Mouse Y") * globalOrbitMousePitchSpeed;
            targetGlobalOrbitPitch = Mathf.Clamp(
                targetGlobalOrbitPitch,
                minGlobalOrbitPitch,
                maxGlobalOrbitPitch
            );
        }

        if (Mathf.Abs(yawDelta) > 0.001f || Input.GetMouseButton(2))
            HadInputThisFrame = true;

        targetYaw += yawDelta;

        currentYaw = Mathf.LerpAngle(
            currentYaw,
            targetYaw,
            1f - Mathf.Exp(-rotationSmoothing * deltaTime)
        );

        GlobalOrbitPitch = Mathf.Lerp(
            GlobalOrbitPitch,
            targetGlobalOrbitPitch,
            1f - Mathf.Exp(-globalOrbitPitchSmoothing * deltaTime)
        );

        rig.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }
}