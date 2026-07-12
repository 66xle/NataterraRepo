using UnityEngine;

[ExecuteAlways]
public class OceanDirectionalLightFollower : MonoBehaviour
{
    [Header("References")]
    public Camera targetCamera;

    [Header("Settings")]
    [Range(-180f, 180f)]
    public float yawOffset = 0f;

    public bool updateInEditMode = true;

    private void Update()
    {
        if (!Application.isPlaying && !updateInEditMode)
            return;

        if (targetCamera == null)
        {
#if UNITY_EDITOR
            targetCamera = UnityEditor.SceneView.lastActiveSceneView != null
                ? UnityEditor.SceneView.lastActiveSceneView.camera
                : Camera.main;
#else
            targetCamera = Camera.main;
#endif
        }

        if (targetCamera == null)
            return;

        Vector3 forward = targetCamera.transform.forward;

        // Ignore camera pitch.
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.0001f)
            return;

        forward.Normalize();

        // Point opposite the camera view.
        float yaw = Quaternion.LookRotation(-forward).eulerAngles.y;

        Vector3 euler = transform.eulerAngles;
        euler.y = yaw + yawOffset;
        transform.eulerAngles = euler;
    }
}