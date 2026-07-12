using UnityEngine;

public class FalseIbisCameraRigSolver : MonoBehaviour
{
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

    [Header("Pivot Feel")]
    [SerializeField] private float lookAheadDistance = 4f;

    [Header("Global Orbit Blend")]
    [Range(0f, 1f)] [SerializeField] private float globalOrbitBlendStartZoom = 0.86f;
    [Range(0f, 1f)] [SerializeField] private float globalOrbitBlendFullZoom = 1f;
    [SerializeField] private AnimationCurve globalOrbitBlendCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private float globalOrbitBlendSmoothing = 3f;
    [SerializeField] private float globalOrbitExitSmoothing = 5f;
    [SerializeField] private bool forcePureOrbitAtFullZoom = true;
    [Range(0f, 1f)] [SerializeField] private float pureOrbitZoomThreshold = 0.985f;
    [SerializeField] private float pureOrbitBlendThreshold = 0.995f;

    [Header("Global Orbit")]
    [SerializeField] private float globalOrbitDistance = 55f;
    [SerializeField] private float globalOrbitHeightOffset = 0f;
    [SerializeField] private float minimumHeightAboveFocus = 4f;

    private float currentOrbitBlend;

    public CameraRigPose Solve(
        Transform rigTransform,
        float zoom,
        FalseIbisCameraMode mode,
        Vector3 focusPoint,
        float globalOrbitPitch,
        float globalOrbitYaw,
        float orbitDistanceOffset,
        float deltaTime)
    {
        CameraRigPose standardPose = SolveStandardRig(rigTransform, zoom);

        CameraRigPose orbitPose = SolveGlobalOrbit(
            focusPoint,
            globalOrbitPitch,
            globalOrbitYaw,
            orbitDistanceOffset
        );

        float rawBlend = Mathf.InverseLerp(
            globalOrbitBlendStartZoom,
            globalOrbitBlendFullZoom,
            zoom
        );

        float targetOrbitBlend =
            globalOrbitBlendCurve.Evaluate(Mathf.Clamp01(rawBlend));

        float smoothing = targetOrbitBlend > currentOrbitBlend
            ? globalOrbitBlendSmoothing
            : globalOrbitExitSmoothing;

        currentOrbitBlend = Mathf.Lerp(
            currentOrbitBlend,
            targetOrbitBlend,
            1f - Mathf.Exp(-smoothing * deltaTime)
        );

        if (forcePureOrbitAtFullZoom && zoom >= pureOrbitZoomThreshold)
        {
            currentOrbitBlend = 1f;
            return orbitPose;
        }

        if (currentOrbitBlend >= pureOrbitBlendThreshold)
            return orbitPose;

        return CameraRigPose.Lerp(standardPose, orbitPose, currentOrbitBlend);
    }

    private CameraRigPose SolveStandardRig(Transform rigTransform, float zoom)
    {
        float height = Mathf.Lerp(closeHeight, farHeight, heightCurve.Evaluate(zoom));
        float distanceBack = Mathf.Lerp(closeDistanceBack, farDistanceBack, distanceCurve.Evaluate(zoom));
        float pitch = Mathf.Lerp(closePitch, farPitch, pitchCurve.Evaluate(zoom));

        Vector3 rigPosition = rigTransform.position;

        Vector3 cameraPosition =
            rigPosition
            - rigTransform.forward * distanceBack
            + Vector3.up * height;

        Vector3 lookTarget =
            rigPosition
            + rigTransform.forward * lookAheadDistance;

        Quaternion lookRotation =
            Quaternion.LookRotation(lookTarget - cameraPosition, Vector3.up);

        Quaternion pitchRotation =
            Quaternion.Euler(pitch, rigTransform.eulerAngles.y, 0f);

        Quaternion cameraRotation =
            Quaternion.Slerp(lookRotation, pitchRotation, 0.25f);

        return new CameraRigPose(cameraPosition, cameraRotation);
    }

    private CameraRigPose SolveGlobalOrbit(
        Vector3 focusPoint,
        float globalOrbitPitch,
        float globalOrbitYaw,
        float orbitDistanceOffset)
    {
        float finalDistance = Mathf.Max(1f, globalOrbitDistance + orbitDistanceOffset);

        float pitchRadians = globalOrbitPitch * Mathf.Deg2Rad;
        float yawRadians = globalOrbitYaw * Mathf.Deg2Rad;

        float horizontalDistance = Mathf.Cos(pitchRadians) * finalDistance;
        float verticalDistance = Mathf.Sin(pitchRadians) * finalDistance;

        Vector3 flatBackDirection = new Vector3(
            -Mathf.Sin(yawRadians),
            0f,
            -Mathf.Cos(yawRadians)
        );

        Vector3 cameraPosition =
            focusPoint
            + flatBackDirection * horizontalDistance
            + Vector3.up * (verticalDistance + globalOrbitHeightOffset);

        float minimumY = focusPoint.y + minimumHeightAboveFocus;

        if (cameraPosition.y < minimumY)
            cameraPosition.y = minimumY;

        Quaternion cameraRotation =
            Quaternion.LookRotation(focusPoint - cameraPosition, Vector3.up);

        return new CameraRigPose(cameraPosition, cameraRotation);
    }
}

public readonly struct CameraRigPose
{
    public readonly Vector3 Position;
    public readonly Quaternion Rotation;

    public CameraRigPose(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public static CameraRigPose Lerp(CameraRigPose a, CameraRigPose b, float t)
    {
        return new CameraRigPose(
            Vector3.Lerp(a.Position, b.Position, t),
            Quaternion.Slerp(a.Rotation, b.Rotation, t)
        );
    }
}