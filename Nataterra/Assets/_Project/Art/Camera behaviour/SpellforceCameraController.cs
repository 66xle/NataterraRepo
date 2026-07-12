using UnityEngine;

public class SpellforceCameraController : MonoBehaviour
{
    private enum NavigationMode { Strategy, Inspect, Drive }

    [Header("References")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Transform startingPosition;
    [SerializeField] private Terrain terrainToFollow;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask collisionMask;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 18f;
    [SerializeField] private float fastMoveMultiplier = 2f;
    [SerializeField] private float movementSmoothing = 10f;

    [Header("Rotation")]
    [SerializeField] private float keyboardRotationSpeed = 90f;
    [SerializeField] private float mouseRotationSpeed = 3f;

    [Header("Zoom Input")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomStepScale = 0.01f;
    [SerializeField] private float zoomSmoothing = 8f;
    [SerializeField] private float zoomVelocityDamping = 12f;

    [Header("Zoom Soft Zones")]
    [SerializeField] private bool useSoftZones = true;
    [SerializeField] private float softZoneWidth = 0.12f;
    [SerializeField] private float softZoneStrength = 0.85f;
    [SerializeField] private AnimationCurve softZoneCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Range(0f, 1f)] [SerializeField] private float lowSoftZone = 0.18f;
    [Range(0f, 1f)] [SerializeField] private float midSoftZone = 0.52f;
    [Range(0f, 1f)] [SerializeField] private float highSoftZone = 0.84f;

    [Header("Navigation Modes")]
    [Range(0f, 1f)] [SerializeField] private float strategyModeZoomThreshold = 0.78f;
    [Range(0f, 1f)] [SerializeField] private float driveModeZoomThreshold = 0.07f;

    [Header("Inspect Mode")]
    [SerializeField] private bool useInspectMode = true;
    [SerializeField] private bool freezeInspectWhileLeftClickHeld = true;
    [SerializeField] private float inspectReturnDelay = 1.25f;
    [Range(0f, 0.95f)] [SerializeField] private float inspectDeadZone = 0.25f;
    [SerializeField] private float inspectMoveStrength = 8f;
    [SerializeField] private float inspectSmoothing = 6f;
    [SerializeField] private float inspectReturnSmoothing = 10f;
    [SerializeField] private float inspectMaxOffset = 14f;
    [SerializeField] private float inspectPitchStrength = 6f;
    [SerializeField] private float inspectYawStrength = 0f;
    [SerializeField] private float inspectRotationSmoothing = 6f;

    [Header("External Focus Target")]
    [SerializeField] private bool useExternalFocusTarget = true;
    [SerializeField] private Transform externalFocusTarget;
    [SerializeField] private Vector3 externalFocusOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private float externalFocusStrength = 0.25f;
    [SerializeField] private float externalFocusSmoothing = 6f;
    [SerializeField] private float externalFocusMaxOffset = 10f;

    [Header("Drive Mode")]
    [SerializeField] private float edgeTurnSpeed = 80f;
    [Range(0f, 1f)] [SerializeField] private float driveYawBlend = 0.98f;
    [Range(0f, 1f)] [SerializeField] private float driveLateralBlend = 0.02f;
    [SerializeField] private float driveMoveSpeedMultiplier = 0.65f;
    [SerializeField] private float drivePitchStrength = 2.5f;
    [SerializeField] private float drivePitchSmoothing = 5f;

    [Header("Swoop Shape")]
    [SerializeField] private AnimationCurve heightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve distanceCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve pitchCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Swoop Values")]
    [SerializeField] private float closeHeight = 3f;
    [SerializeField] private float farHeight = 30f;
    [SerializeField] private float closeDistanceBack = 5f;
    [SerializeField] private float farDistanceBack = 22f;
    [SerializeField] private float closePitch = 12f;
    [SerializeField] private float farPitch = 68f;

    [Header("FOV")]
    [SerializeField] private bool useFovCurve = true;
    [SerializeField] private float closeFov = 58f;
    [SerializeField] private float farFov = 42f;
    [SerializeField] private AnimationCurve fovCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private float fovSmoothing = 8f;

    [Header("Pivot Feel")]
    [SerializeField] private float lookAheadDistance = 4f;

    [Header("Terrain Following")]
    [SerializeField] private bool followTerrainHeight = true;
    [SerializeField] private float heightAboveGround = 0f;
    [SerializeField] private float terrainFollowSmoothing = 8f;

    [Header("Collision")]
    [SerializeField] private bool preventCameraClipping = true;
    [SerializeField] private float cameraCollisionRadius = 0.35f;
    [SerializeField] private float collisionPadding = 0.35f;
    [SerializeField] private float minimumCameraGroundClearance = 1.2f;

    [Header("Edge Scroll")]
    [SerializeField] private bool useEdgeScroll = true;
    [SerializeField] private float edgeSize = 16f;

    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;

    private Vector3 targetPivotPosition;
    private Vector3 inspectOffset;
    private Vector3 targetInspectOffset;

    private Vector3 externalFocusCameraOffset;
    private Vector3 targetExternalFocusCameraOffset;

    private float targetZoom = 0.5f;
    private float currentZoom = 0.5f;
    private float zoomVelocity;
    private float currentGroundHeight;

    private float inspectPitchOffset;
    private float targetInspectPitchOffset;
    private float inspectYawOffset;
    private float targetInspectYawOffset;
    private float inspectReturnTimer;

    private float drivePitchOffset;
    private float targetDrivePitchOffset;

    private bool movementInputThisFrame;
    private bool cameraActionThisFrame;

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (startingPosition != null)
        {
            transform.position = startingPosition.position;
            transform.rotation = startingPosition.rotation;
        }

        targetPivotPosition = transform.position;
        currentGroundHeight = transform.position.y;
    }

    private void Update()
    {
        movementInputThisFrame = false;
        cameraActionThisFrame = false;

        HandleZoom();
        HandleRotation();
        HandleMovement();
        HandleInspectMode();
        HandleExternalFocusTarget();
        HandleDrivePitch();
    }

    private void LateUpdate()
    {
        ApplyMovement();
        ApplyTerrainHeight();
        ApplyCameraRig();
    }

    public void SetExternalFocusTarget(Transform target)
    {
        externalFocusTarget = target;
    }

    public void ClearExternalFocusTarget()
    {
        externalFocusTarget = null;
    }

    private NavigationMode GetNavigationMode()
    {
        if (currentZoom <= driveModeZoomThreshold)
            return NavigationMode.Drive;

        if (currentZoom >= strategyModeZoomThreshold)
            return NavigationMode.Strategy;

        return NavigationMode.Inspect;
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            float direction = -Mathf.Sign(scroll);
            zoomVelocity += direction * zoomSpeed * zoomStepScale;
            cameraActionThisFrame = true;
        }

        float resistance = useSoftZones ? GetSoftZoneResistance(targetZoom) : 1f;
        zoomVelocity *= resistance;

        targetZoom += zoomVelocity;
        targetZoom = Mathf.Clamp01(targetZoom);

        zoomVelocity = Mathf.Lerp(
            zoomVelocity,
            0f,
            1f - Mathf.Exp(-zoomVelocityDamping * Time.deltaTime)
        );
    }

    private float GetSoftZoneResistance(float zoom)
    {
        float resistance = 1f;

        resistance = Mathf.Min(resistance, GetSingleSoftZoneResistance(zoom, lowSoftZone));
        resistance = Mathf.Min(resistance, GetSingleSoftZoneResistance(zoom, midSoftZone));
        resistance = Mathf.Min(resistance, GetSingleSoftZoneResistance(zoom, highSoftZone));

        return resistance;
    }

    private float GetSingleSoftZoneResistance(float zoom, float zone)
    {
        float distance = Mathf.Abs(zoom - zone);

        if (distance >= softZoneWidth)
            return 1f;

        float t = distance / softZoneWidth;
        float curveValue = softZoneCurve.Evaluate(t);

        return Mathf.Lerp(1f - softZoneStrength, 1f, curveValue);
    }

    private void HandleRotation()
    {
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.Q)) rotationInput -= 1f;
        if (Input.GetKey(KeyCode.E)) rotationInput += 1f;

        if (Mathf.Abs(rotationInput) > 0.01f)
            cameraActionThisFrame = true;

        transform.Rotate(Vector3.up, rotationInput * keyboardRotationSpeed * Time.deltaTime, Space.World);

        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");

            if (Mathf.Abs(mouseX) > 0.01f)
                cameraActionThisFrame = true;

            transform.Rotate(Vector3.up, mouseX * mouseRotationSpeed, Space.World);
        }
    }

    private void HandleMovement()
    {
        NavigationMode mode = GetNavigationMode();

        Vector3 input = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) input += Vector3.forward;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) input += Vector3.back;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) input += Vector3.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) input += Vector3.right;

        if (useEdgeScroll)
        {
            if (mode == NavigationMode.Strategy)
                input += GetStrategyEdgeInput();
            else if (mode == NavigationMode.Drive)
                input += HandleDriveEdgeInput();
        }

        if (input.sqrMagnitude > 0.001f)
        {
            movementInputThisFrame = true;
            cameraActionThisFrame = true;
        }

        if (input.sqrMagnitude > 1f)
            input.Normalize();

        float speed = Input.GetKey(KeyCode.LeftShift)
            ? moveSpeed * fastMoveMultiplier
            : moveSpeed;

        if (mode == NavigationMode.Drive)
            speed *= driveMoveSpeedMultiplier;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * input.z + right * input.x;

        if (moveDirection.sqrMagnitude > 0.001f)
            targetPivotPosition += moveDirection.normalized * speed * Time.deltaTime;
    }

    private Vector3 GetStrategyEdgeInput()
    {
        Vector3 input = Vector3.zero;
        Vector3 mouse = Input.mousePosition;

        if (mouse.x <= edgeSize) input += Vector3.left;
        else if (mouse.x >= Screen.width - edgeSize) input += Vector3.right;

        if (mouse.y <= edgeSize) input += Vector3.back;
        else if (mouse.y >= Screen.height - edgeSize) input += Vector3.forward;

        return input;
    }

    private Vector3 HandleDriveEdgeInput()
    {
        Vector3 input = Vector3.zero;
        Vector3 mouse = Input.mousePosition;

        bool leftEdge = mouse.x <= edgeSize;
        bool rightEdge = mouse.x >= Screen.width - edgeSize;
        bool bottomEdge = mouse.y <= edgeSize;
        bool topEdge = mouse.y >= Screen.height - edgeSize;

        float turnInput = 0f;

        if (leftEdge) turnInput -= 1f;
        else if (rightEdge) turnInput += 1f;

        if (Mathf.Abs(turnInput) > 0.01f)
            cameraActionThisFrame = true;

        transform.Rotate(
            Vector3.up,
            turnInput * edgeTurnSpeed * driveYawBlend * Time.deltaTime,
            Space.World
        );

        if (leftEdge) input += Vector3.left * driveLateralBlend;
        else if (rightEdge) input += Vector3.right * driveLateralBlend;

        if (bottomEdge) input += Vector3.back;
        else if (topEdge) input += Vector3.forward;

        return input;
    }

    private void HandleInspectMode()
    {
        bool inInspectMode = useInspectMode && GetNavigationMode() == NavigationMode.Inspect;
        bool frozenByClick = freezeInspectWhileLeftClickHeld && Input.GetMouseButton(0);

        if (!inInspectMode)
        {
            targetInspectOffset = Vector3.zero;
            targetInspectPitchOffset = 0f;
            targetInspectYawOffset = 0f;
            SmoothInspectValues(inspectReturnSmoothing);
            return;
        }

        if (movementInputThisFrame || cameraActionThisFrame && !Input.GetMouseButton(0))
            inspectReturnTimer = 0f;

        if (frozenByClick)
        {
            targetInspectOffset = inspectOffset;
            targetInspectPitchOffset = inspectPitchOffset;
            targetInspectYawOffset = inspectYawOffset;
            SmoothInspectValues(inspectSmoothing);
            return;
        }

        Vector2 mouseNormalised = new Vector2(
            (Input.mousePosition.x / Screen.width) * 2f - 1f,
            (Input.mousePosition.y / Screen.height) * 2f - 1f
        );

        Vector2 deadzoned = ApplyDeadZone(mouseNormalised, inspectDeadZone);
        bool hasInspectInput = deadzoned.sqrMagnitude > 0.001f;

        if (hasInspectInput)
        {
            inspectReturnTimer = 0f;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            targetInspectOffset =
                (right * deadzoned.x + forward * deadzoned.y) * inspectMoveStrength;

            targetInspectOffset = Vector3.ClampMagnitude(targetInspectOffset, inspectMaxOffset);

            targetInspectPitchOffset = -deadzoned.y * inspectPitchStrength;
            targetInspectYawOffset = deadzoned.x * inspectYawStrength;

            SmoothInspectValues(inspectSmoothing);
        }
        else
        {
            inspectReturnTimer += Time.deltaTime;

            if (inspectReturnTimer >= inspectReturnDelay || cameraActionThisFrame)
            {
                targetInspectOffset = Vector3.zero;
                targetInspectPitchOffset = 0f;
                targetInspectYawOffset = 0f;
            }

            SmoothInspectValues(inspectReturnSmoothing);
        }
    }

    private void HandleExternalFocusTarget()
    {
        if (!useExternalFocusTarget || externalFocusTarget == null)
        {
            targetExternalFocusCameraOffset = Vector3.zero;
        }
        else
        {
            Vector3 focusPoint = externalFocusTarget.position + externalFocusOffset;
            Vector3 worldDelta = focusPoint - transform.position;
            worldDelta.y = 0f;

            targetExternalFocusCameraOffset = Vector3.ClampMagnitude(
                worldDelta * externalFocusStrength,
                externalFocusMaxOffset
            );
        }

        externalFocusCameraOffset = Vector3.Lerp(
            externalFocusCameraOffset,
            targetExternalFocusCameraOffset,
            1f - Mathf.Exp(-externalFocusSmoothing * Time.deltaTime)
        );
    }

    private void HandleDrivePitch()
    {
        if (GetNavigationMode() != NavigationMode.Drive)
        {
            targetDrivePitchOffset = 0f;
        }
        else
        {
            float pitchInput = 0f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                pitchInput -= 1f;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                pitchInput += 1f;

            if (useEdgeScroll)
            {
                Vector3 mouse = Input.mousePosition;

                if (mouse.y >= Screen.height - edgeSize)
                    pitchInput -= 1f;
                else if (mouse.y <= edgeSize)
                    pitchInput += 1f;
            }

            pitchInput = Mathf.Clamp(pitchInput, -1f, 1f);
            targetDrivePitchOffset = pitchInput * drivePitchStrength;
        }

        drivePitchOffset = Mathf.Lerp(
            drivePitchOffset,
            targetDrivePitchOffset,
            1f - Mathf.Exp(-drivePitchSmoothing * Time.deltaTime)
        );
    }

    private void SmoothInspectValues(float smoothing)
    {
        inspectOffset = Vector3.Lerp(
            inspectOffset,
            targetInspectOffset,
            1f - Mathf.Exp(-smoothing * Time.deltaTime)
        );

        inspectPitchOffset = Mathf.Lerp(
            inspectPitchOffset,
            targetInspectPitchOffset,
            1f - Mathf.Exp(-inspectRotationSmoothing * Time.deltaTime)
        );

        inspectYawOffset = Mathf.Lerp(
            inspectYawOffset,
            targetInspectYawOffset,
            1f - Mathf.Exp(-inspectRotationSmoothing * Time.deltaTime)
        );
    }

    private Vector2 ApplyDeadZone(Vector2 input, float deadZone)
    {
        float magnitude = input.magnitude;

        if (magnitude <= deadZone)
            return Vector2.zero;

        float adjustedMagnitude = Mathf.InverseLerp(deadZone, 1f, magnitude);
        return input.normalized * adjustedMagnitude;
    }

    private void ApplyMovement()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPivotPosition,
            1f - Mathf.Exp(-movementSmoothing * Time.deltaTime)
        );
    }

    private void ApplyTerrainHeight()
    {
        if (!followTerrainHeight)
            return;

        float desiredGroundHeight = transform.position.y;

        if (terrainToFollow != null)
        {
            desiredGroundHeight =
                terrainToFollow.SampleHeight(transform.position)
                + terrainToFollow.transform.position.y;
        }
        else
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 200f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 500f, groundMask))
                desiredGroundHeight = hit.point.y;
        }

        currentGroundHeight = Mathf.Lerp(
            currentGroundHeight,
            desiredGroundHeight + heightAboveGround,
            1f - Mathf.Exp(-terrainFollowSmoothing * Time.deltaTime)
        );

        Vector3 position = transform.position;
        position.y = currentGroundHeight;

        transform.position = position;
        targetPivotPosition.y = currentGroundHeight;
    }

    private void ApplyCameraRig()
    {
        currentZoom = Mathf.Lerp(
            currentZoom,
            targetZoom,
            1f - Mathf.Exp(-zoomSmoothing * Time.deltaTime)
        );

        float height = Mathf.Lerp(closeHeight, farHeight, heightCurve.Evaluate(currentZoom));
        float distanceBack = Mathf.Lerp(closeDistanceBack, farDistanceBack, distanceCurve.Evaluate(currentZoom));
        float pitch = Mathf.Lerp(closePitch, farPitch, pitchCurve.Evaluate(currentZoom));

        Vector3 rigPosition = transform.position + inspectOffset + externalFocusCameraOffset;

        Vector3 lookTarget =
            rigPosition + transform.forward * lookAheadDistance;

        Vector3 desiredCameraPosition =
            rigPosition
            - transform.forward * distanceBack
            + Vector3.up * height;

        if (preventCameraClipping)
            desiredCameraPosition = ResolveCameraCollision(lookTarget, desiredCameraPosition);

        desiredCameraPosition = ClampCameraAboveTerrain(desiredCameraPosition);

        Quaternion lookRotation =
            Quaternion.LookRotation(lookTarget - desiredCameraPosition, Vector3.up);

        Quaternion pitchRotation =
            Quaternion.Euler(
                pitch + inspectPitchOffset + drivePitchOffset,
                transform.eulerAngles.y + inspectYawOffset,
                0f
            );

        Quaternion desiredRotation =
            Quaternion.Slerp(lookRotation, pitchRotation, 0.25f);

        targetCamera.transform.position = desiredCameraPosition;
        targetCamera.transform.rotation = desiredRotation;

        ApplyFov();
    }

    private Vector3 ResolveCameraCollision(Vector3 lookTarget, Vector3 desiredCameraPosition)
    {
        Vector3 castDirection = desiredCameraPosition - lookTarget;
        float castDistance = castDirection.magnitude;

        if (castDistance <= 0.01f)
            return desiredCameraPosition;

        castDirection.Normalize();

        if (Physics.SphereCast(
            lookTarget,
            cameraCollisionRadius,
            castDirection,
            out RaycastHit hit,
            castDistance,
            collisionMask,
            QueryTriggerInteraction.Ignore))
        {
            return hit.point - castDirection * collisionPadding;
        }

        return desiredCameraPosition;
    }

    private Vector3 ClampCameraAboveTerrain(Vector3 cameraPosition)
    {
        float groundHeight = cameraPosition.y;

        if (terrainToFollow != null)
        {
            groundHeight =
                terrainToFollow.SampleHeight(cameraPosition)
                + terrainToFollow.transform.position.y;
        }
        else
        {
            Vector3 rayOrigin = cameraPosition + Vector3.up * 200f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 500f, groundMask))
                groundHeight = hit.point.y;
        }

        float minimumY = groundHeight + minimumCameraGroundClearance;

        if (cameraPosition.y < minimumY)
            cameraPosition.y = minimumY;

        return cameraPosition;
    }

    private void ApplyFov()
    {
        if (!useFovCurve || targetCamera == null)
            return;

        float fovT = fovCurve.Evaluate(currentZoom);
        float targetFov = Mathf.Lerp(closeFov, farFov, fovT);

        targetCamera.fieldOfView = Mathf.Lerp(
            targetCamera.fieldOfView,
            targetFov,
            1f - Mathf.Exp(-fovSmoothing * Time.deltaTime)
        );
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.DrawWireSphere(transform.position, 0.35f);

        Vector3 lookTarget = transform.position + transform.forward * lookAheadDistance;
        Gizmos.DrawWireSphere(lookTarget, 0.25f);

        if (targetCamera != null)
            Gizmos.DrawLine(lookTarget, targetCamera.transform.position);

        if (externalFocusTarget != null)
            Gizmos.DrawWireSphere(externalFocusTarget.position + externalFocusOffset, 0.3f);
    }
}