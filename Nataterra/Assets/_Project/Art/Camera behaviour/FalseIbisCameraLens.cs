using UnityEngine;

public class FalseIbisCameraLens : MonoBehaviour
{
    [Header("Lens")]
    [SerializeField] private bool useFovCurve = true;
    [SerializeField] private float closeFov = 58f;
    [SerializeField] private float farFov = 42f;
    [SerializeField] private AnimationCurve fovCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private float fovSmoothing = 8f;

    [Header("Safety")]
    [SerializeField] private bool forcePhysicalCameraOff = true;
    [SerializeField] private bool logFovDebug = false;

    public void Apply(Camera targetCamera, float zoom, float deltaTime)
    {
        if (targetCamera == null)
            return;

        if (forcePhysicalCameraOff)
            targetCamera.usePhysicalProperties = false;

        if (!useFovCurve)
            return;

        float fovT = fovCurve.Evaluate(zoom);
        float targetFov = Mathf.Lerp(closeFov, farFov, fovT);

        targetCamera.fieldOfView = Mathf.Lerp(
            targetCamera.fieldOfView,
            targetFov,
            1f - Mathf.Exp(-fovSmoothing * deltaTime)
        );

        if (logFovDebug)
        {
            Debug.Log(
                $"Zoom: {zoom:F3} | FOV T: {fovT:F3} | Target FOV: {targetFov:F2} | Actual FOV: {targetCamera.fieldOfView:F2}"
            );
        }
    }
}