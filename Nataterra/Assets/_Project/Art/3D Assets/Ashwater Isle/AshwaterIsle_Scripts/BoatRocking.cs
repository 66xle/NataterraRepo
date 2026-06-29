using UnityEngine;

public class BoatRocking : MonoBehaviour
{
    public enum TiltAxis { X, Y, Z }

    public enum ObjectScale
    {
        Tiny,
        Small,
        Medium,
        Large,
        Huge,
        Custom
    }

    [Header("Scale Profile")]
    [SerializeField] private ObjectScale objectScale = ObjectScale.Medium;
    [SerializeField] private bool useScaleProfile = true;

    [Header("Position Motion")]
    [SerializeField] private float positionSpeed = 0.35f;
    [SerializeField] private Vector3 positionAmount = new Vector3(0.05f, 0.12f, 0.05f);

    [Header("Tilt Motion")]
    [SerializeField] private float tiltSpeed = 0.45f;
    [SerializeField] private float tiltAmount = 4f;
    [SerializeField] private TiltAxis tiltAxis = TiltAxis.X;

    [Header("Secondary Tilt")]
    [SerializeField] private bool useSecondaryTilt = true;
    [SerializeField] private TiltAxis secondaryTiltAxis = TiltAxis.Z;
    [SerializeField] private float secondaryTiltAmount = 2f;
    [SerializeField] private float secondaryTiltSpeed = 0.3f;

    [Header("Random Phase")]
    [SerializeField] private bool randomisePhase = true;
    [SerializeField] private float maxPhaseOffset = 1000f;

    [Header("Profile Multipliers")]
    [SerializeField] private float profilePositionMultiplier = 1f;
    [SerializeField] private float profileTiltMultiplier = 1f;
    [SerializeField] private float profileSpeedMultiplier = 1f;

    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;

    private float positionPhase;
    private float rotationPhase;
    private float secondaryRotationPhase;

    private void Awake()
    {
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;

        ApplyScaleProfile();

        if (randomisePhase)
        {
            positionPhase = Random.Range(0f, maxPhaseOffset);
            rotationPhase = Random.Range(0f, maxPhaseOffset);
            secondaryRotationPhase = Random.Range(0f, maxPhaseOffset);
        }
    }

    private void Update()
    {
        ApplyPositionMotion();
        ApplyTiltMotion();
    }

    private void ApplyPositionMotion()
    {
        float speed = positionSpeed * profileSpeedMultiplier;
        float t = Time.time * speed + positionPhase;

        Vector3 offset = new Vector3(
            Noise(t + 10f) * positionAmount.x * profilePositionMultiplier,
            Noise(t + 20f) * positionAmount.y * profilePositionMultiplier,
            Noise(t + 30f) * positionAmount.z * profilePositionMultiplier
        );

        transform.localPosition = startLocalPosition + offset;
    }

    private void ApplyTiltMotion()
    {
        Vector3 rotation = Vector3.zero;

        float primary = Noise(Time.time * tiltSpeed * profileSpeedMultiplier + rotationPhase) 
                        * tiltAmount 
                        * profileTiltMultiplier;

        AddRotation(ref rotation, tiltAxis, primary);

        if (useSecondaryTilt)
        {
            float secondary = Noise(Time.time * secondaryTiltSpeed * profileSpeedMultiplier + secondaryRotationPhase) 
                              * secondaryTiltAmount 
                              * profileTiltMultiplier;

            AddRotation(ref rotation, secondaryTiltAxis, secondary);
        }

        transform.localRotation = startLocalRotation * Quaternion.Euler(rotation);
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