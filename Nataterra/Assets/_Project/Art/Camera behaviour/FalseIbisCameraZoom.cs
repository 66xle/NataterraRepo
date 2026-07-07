using UnityEngine;

public class FalseIbisCameraZoom : MonoBehaviour
{
    [Header("Zoom Input")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomStepScale = 0.01f;
    [SerializeField] private float zoomSmoothing = 8f;
    [SerializeField] private float zoomVelocityDamping = 12f;

    [Header("Zoom Soft Zones")]
    [SerializeField] private bool useSoftZones = true;
    [SerializeField] private float softZoneWidth = 0.12f;
    [SerializeField] private float softZoneStrength = 0.65f;
    [SerializeField] private AnimationCurve softZoneCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Range(0f, 1f)] [SerializeField] private float lowSoftZone = 0.18f;
    [Range(0f, 1f)] [SerializeField] private float midSoftZone = 0.52f;
    [Range(0f, 1f)] [SerializeField] private float highSoftZone = 0.84f;

    public float TargetZoom { get; private set; } = 0.5f;
    public float CurrentZoom { get; private set; } = 0.5f;

    public bool HadInputThisFrame { get; private set; }

    private float zoomVelocity;

    public void Tick(float deltaTime)
    {
        HadInputThisFrame = false;

        HandleInput();
        ApplyZoom(deltaTime);
    }

    private void HandleInput()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) <= 0.01f)
            return;

        float direction = -Mathf.Sign(scroll);

        zoomVelocity += direction * zoomSpeed * zoomStepScale;
        HadInputThisFrame = true;
    }

    private void ApplyZoom(float deltaTime)
    {
        float resistance = useSoftZones ? GetSoftZoneResistance(TargetZoom) : 1f;

        zoomVelocity *= resistance;

        TargetZoom += zoomVelocity;
        TargetZoom = Mathf.Clamp01(TargetZoom);

        zoomVelocity = Mathf.Lerp(
            zoomVelocity,
            0f,
            1f - Mathf.Exp(-zoomVelocityDamping * deltaTime)
        );

        CurrentZoom = Mathf.Lerp(
            CurrentZoom,
            TargetZoom,
            1f - Mathf.Exp(-zoomSmoothing * deltaTime)
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

    public void SetZoomImmediate(float zoom)
    {
        TargetZoom = Mathf.Clamp01(zoom);
        CurrentZoom = TargetZoom;
        zoomVelocity = 0f;
    }
}