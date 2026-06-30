using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private WaterWaveSettings waveSettings;

    [Header("Floating")]
    [SerializeField] private float heightStrength = 1f;
    [SerializeField] private float verticalOffset = 0f;

    [Header("Tilt Sampling")]
    [SerializeField] private bool alignToWaterSlope = true;
    [SerializeField] private float sampleDistance = 1f;
    [SerializeField] private float tiltStrength = 1f;

    [Header("Smoothing")]
    [SerializeField] private float positionSmoothing = 4f;
    [SerializeField] private float rotationSmoothing = 3f;

    [Header("Extra Life")]
    [SerializeField] private bool useSecondaryWobble = true;
    [SerializeField] private float wobbleAmount = 1f;
    [SerializeField] private float wobbleSpeed = 0.4f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float phaseOffset;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        phaseOffset = Random.Range(0f, 1000f);
    }

    private void Update()
    {
        if (waveSettings == null)
            return;

        float time = Time.time;

        ApplyFloating(time);
        ApplyRotation(time);
    }

    private void ApplyFloating(float time)
    {
        float waterHeight = waveSettings.GetHeight(transform.position, time);

        Vector3 targetPosition = startPosition;
        targetPosition.y += waterHeight * heightStrength + verticalOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * positionSmoothing
        );
    }

    private void ApplyRotation(float time)
    {
        Quaternion targetRotation = startRotation;

        if (alignToWaterSlope)
        {
            Vector3 forwardPoint = transform.position + transform.forward * sampleDistance;
            Vector3 backPoint = transform.position - transform.forward * sampleDistance;
            Vector3 rightPoint = transform.position + transform.right * sampleDistance;
            Vector3 leftPoint = transform.position - transform.right * sampleDistance;

            float forwardHeight = waveSettings.GetHeight(forwardPoint, time);
            float backHeight = waveSettings.GetHeight(backPoint, time);
            float rightHeight = waveSettings.GetHeight(rightPoint, time);
            float leftHeight = waveSettings.GetHeight(leftPoint, time);

            float pitch = (backHeight - forwardHeight) * tiltStrength;
            float roll = (leftHeight - rightHeight) * tiltStrength;

            targetRotation = startRotation * Quaternion.Euler(
                pitch,
                0f,
                roll
            );
        }

        if (useSecondaryWobble)
        {
            float wobble = Mathf.PerlinNoise(Time.time * wobbleSpeed + phaseOffset, 0f) * 2f - 1f;
            targetRotation *= Quaternion.Euler(0f, 0f, wobble * wobbleAmount);
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSmoothing
        );
    }
}