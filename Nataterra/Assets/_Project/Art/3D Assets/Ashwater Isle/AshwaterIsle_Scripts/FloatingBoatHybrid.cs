using UnityEngine;

public class FloatingBoatHybrid : MonoBehaviour
{
    public enum TiltAxis { X, Y, Z }
    public enum ObjectScale { Tiny, Small, Medium, Large, Huge, Custom }

    [Header("Water Height Sync")]
    [SerializeField] private WaterWaveSettings waveSettings;
    [SerializeField] private bool syncHeightToWater = true;
    [SerializeField] private float waterHeightStrength = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private float positionSmoothing = 4f;

    [Header("Scale Profile")]
    [SerializeField] private ObjectScale objectScale = ObjectScale.Medium;
    [SerializeField] private bool useScaleProfile = true;

    [Header("Artistic Local Drift")]
    [SerializeField] private bool useLocalDrift = true;
    [SerializeField] private float driftSpeed = 0.25f;
    [SerializeField] private Vector3 driftAmount = new Vector3(0.03f, 0f, 0.03f);

    [Header("Artistic Tilt")]
    [SerializeField] private bool useArtisticTilt = true;
    [SerializeField] private float tiltSpeed = 0.45f;
    [SerializeField] private float tiltAmount = 4f;
    [SerializeField] private TiltAxis tiltAxis = TiltAxis.X;

    [Header("Secondary Tilt")]
    [SerializeField] private bool useSecondaryTilt = true;
    [SerializeField] private TiltAxis secondaryTiltAxis = TiltAxis.Z;
    [SerializeField] private float secondaryTiltAmount = 2f;
    [SerializeField] private float secondaryTiltSpeed = 0.3f;

    [Header("Optional Water Slope Influence")]
    [SerializeField] private bool useWaterSlopeTilt = false;
    [SerializeField] private float sampleDistance = 2f;
    [SerializeField] private float waterSlopeTiltStrength = 2f;

    [Header("Random Phase")]
    [SerializeField] private bool randomisePhase = true;
    [SerializeField] private float maxPhaseOffset = 1000f;

    [Header("Profile Multipliers")]
    [SerializeField] private float profilePositionMultiplier = 1f;
    [SerializeField] private float profileTiltMultiplier = 1f;
    [SerializeField] private float profileSpeedMultiplier = 1f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private float driftPhase;
    private float primaryTiltPhase;
    private float secondaryTiltPhase;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        ApplyScaleProfile();

        if (randomisePhase)
        {
            driftPhase = Random.Range(0f, maxPhaseOffset);
            primaryTiltPhase = Random.Range(0f, maxPhaseOffset);
            secondaryTiltPhase = Random.Range(0f, maxPhaseOffset);
        }
    }

    private void Update()
    {
        float time = Time.time;

        ApplyPosition(time);
        ApplyRotation(time);
    }

    private void ApplyPosition(float time)
    {
        Vector3 targetPosition = startPosition;

        if (syncHeightToWater && waveSettings != null)
        {
            float waterHeight = waveSettings.GetHeight(startPosition, time);
            targetPosition.y += waterHeight * waterHeightStrength + verticalOffset;
        }

        if (useLocalDrift)
        {
            float t = time * driftSpeed * profileSpeedMultiplier + driftPhase;

            targetPosition.x += Noise(t + 10f) * driftAmount.x * profilePositionMultiplier;
            targetPosition.y += Noise(t + 20f) * driftAmount.y * profilePositionMultiplier;
            targetPosition.z += Noise(t + 30f) * driftAmount.z * profilePositionMultiplier;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * positionSmoothing
        );
    }

    private void ApplyRotation(float time)
    {
        Vector3 rotationOffset = Vector3.zero;

        if (useArtisticTilt)
        {
            float primary = Noise(time * tiltSpeed * profileSpeedMultiplier + primaryTiltPhase)
                            * tiltAmount
                            * profileTiltMultiplier;

            AddRotation(ref rotationOffset, tiltAxis, primary);
        }

        if (useSecondaryTilt)
        {
            float secondary = Noise(time * secondaryTiltSpeed * profileSpeedMultiplier + secondaryTiltPhase)
                              * secondaryTiltAmount
                              * profileTiltMultiplier;

            AddRotation(ref rotationOffset, secondaryTiltAxis, secondary);
        }

        if (useWaterSlopeTilt && waveSettings != null)
        {
            AddWaterSlopeTilt(ref rotationOffset, time);
        }

        Quaternion targetRotation = startRotation * Quaternion.Euler(rotationOffset);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * positionSmoothing
        );
    }

    private void AddWaterSlopeTilt(ref Vector3 rotationOffset, float time)
    {
        Vector3 forwardPoint = startPosition + transform.forward * sampleDistance;
        Vector3 backPoint = startPosition - transform.forward * sampleDistance;
        Vector3 rightPoint = startPosition + transform.right * sampleDistance;
        Vector3 leftPoint = startPosition - transform.right * sampleDistance;

        float forwardHeight = waveSettings.GetHeight(forwardPoint, time);
        float backHeight = waveSettings.GetHeight(backPoint, time);
        float rightHeight = waveSettings.GetHeight(rightPoint, time);
        float leftHeight = waveSettings.GetHeight(leftPoint, time);

        float pitch = (backHeight - forwardHeight) * waterSlopeTiltStrength;
        float roll = (leftHeight - rightHeight) * waterSlopeTiltStrength;

        rotationOffset.x += pitch;
        rotationOffset.z += roll;
    }

    private void ApplyScaleProfile()
    {
        if (!useScaleProfile || objectScale == ObjectScale.Custom)
            return;

        switch (objectScale)
        {
            case ObjectScale.Tiny:
                profilePositionMultiplier = 1.4f;
                profileTiltMultiplier = 1.6f;
                profileSpeedMultiplier = 1.4f;
                break;

            case ObjectScale.Small:
                profilePositionMultiplier = 1.15f;
                profileTiltMultiplier = 1.25f;
                profileSpeedMultiplier = 1.15f;
                break;

            case ObjectScale.Medium:
                profilePositionMultiplier = 1f;
                profileTiltMultiplier = 1f;
                profileSpeedMultiplier = 1f;
                break;

            case ObjectScale.Large:
                profilePositionMultiplier = 0.75f;
                profileTiltMultiplier = 0.65f;
                profileSpeedMultiplier = 0.75f;
                break;

            case ObjectScale.Huge:
                profilePositionMultiplier = 0.5f;
                profileTiltMultiplier = 0.4f;
                profileSpeedMultiplier = 0.55f;
                break;
        }
    }

    private float Noise(float t)
    {
        return Mathf.PerlinNoise(t, 0f) * 2f - 1f;
    }

    private void AddRotation(ref Vector3 rotation, TiltAxis axis, float amount)
    {
        switch (axis)
        {
            case TiltAxis.X:
                rotation.x += amount;
                break;
            case TiltAxis.Y:
                rotation.y += amount;
                break;
            case TiltAxis.Z:
                rotation.z += amount;
                break;
        }
    }
}