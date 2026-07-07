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

    public CameraRigPose Solve(Transform rigTransform, float zoom)
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
}