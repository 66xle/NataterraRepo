using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class HDRPWaterFloatAnchor : MonoBehaviour
{
    [Header("Editor Preview")]
    [SerializeField] private bool runInEditor = false;

    [Header("HDRP Water")]
    [SerializeField] private WaterSurface waterSurface;
    [SerializeField] private bool adjustHeight = true;
    [SerializeField] private float waterHeightOffset = 0f;
    [SerializeField] private float heightSmooth = 8f;

    [Header("Water Contact")]
    [SerializeField] private bool onlyAffectWhenWaterReachesObject = false;
    [SerializeField] private float waterContactHeightOffset = 0f;
    [SerializeField] private float waterContactFadeDistance = 0.25f;

    [Header("Surface Samples")]
    [SerializeField] private float forwardSampleDistance = 2f;
    [SerializeField] private float sideSampleDistance = 1f;

    [Header("Surface Tilt")]
    [SerializeField] private bool alignToWaterSurface = true;
    [SerializeField] private float rotationSmooth = 5f;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    [SerializeField] private float maxPitchAngle = 20f;
    [SerializeField] private float maxRollAngle = 20f;

    [Header("Wave Slope Drift")]
    [SerializeField] private bool useWaveSlopeDrift = true;
    [SerializeField] private float waveDriftStrength = 0.8f;
    [SerializeField] private float waveDriftMaxSpeed = 0.35f;
    [SerializeField] private float waveDriftDamping = 1.2f;

    [Header("Random Drift")]
    [SerializeField] private bool useRandomDrift = false;
    [SerializeField] private float randomDriftStrength = 0.08f;
    [SerializeField] private float randomDriftSpeed = 0.12f;
    [SerializeField] private float randomDriftMaxSpeed = 0.12f;
    [SerializeField] private float randomDriftDamping = 1.5f;
    [SerializeField] private bool randomiseDriftPhase = true;

    [Header("Anchor")]
    [SerializeField] private bool useAnchor = true;
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private bool resetAnchorOnEnable = true;
    [SerializeField] private float anchorRadius = 2f;
    [SerializeField] private float anchorPullStrength = 0.8f;
    [SerializeField] private float anchorMaxSpeed = 0.4f;
    [SerializeField] private float anchorDamping = 1.2f;

    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;

    private Vector3 anchorPosition;
    private Vector3 waveVelocity;
    private Vector3 randomVelocity;
    private Vector3 anchorVelocity;

    private float randomDriftPhase;
    private float waterContactWeight = 1f;

    private bool hasCenterSample;
    private bool hasForwardSample;
    private bool hasSideSample;

    private WaterSearchParameters centerParams = new WaterSearchParameters();
    private WaterSearchParameters forwardParams = new WaterSearchParameters();
    private WaterSearchParameters sideParams = new WaterSearchParameters();

    private WaterSearchResult centerResult = new WaterSearchResult();
    private WaterSearchResult forwardResult = new WaterSearchResult();
    private WaterSearchResult sideResult = new WaterSearchResult();

    private void OnEnable()
    {
        if (resetAnchorOnEnable || anchorPosition == Vector3.zero)
            SetAnchorHere();

        if (randomiseDriftPhase)
            randomDriftPhase = Random.Range(0f, 1000f);

#if UNITY_EDITOR
        if (!Application.isPlaying)
            EditorApplication.update += EditorTick;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= EditorTick;
#endif
    }

#if UNITY_EDITOR
    private void EditorTick()
    {
        if (!Application.isPlaying && runInEditor)
        {
            Tick();
            SceneView.RepaintAll();
        }
    }
#endif

    private void Update()
    {
        if (!Application.isPlaying && !runInEditor)
            return;

        Tick();
    }

    [ContextMenu("Set Anchor Here")]
    public void SetAnchorHere()
    {
        anchorPosition = anchorPoint != null ? anchorPoint.position : transform.position;

        waveVelocity = Vector3.zero;
        randomVelocity = Vector3.zero;
        anchorVelocity = Vector3.zero;
    }

    private void Tick()
    {
        if (waterSurface == null)
            return;

        float dt = GetDeltaTime();

        SampleWater();

        if (!hasCenterSample)
            return;

        UpdateWaterContactWeight();

        if (useWaveSlopeDrift && hasForwardSample && hasSideSample)
            ApplyWaveSlopeDrift(dt, waterContactWeight);

        if (useRandomDrift)
            ApplyRandomDrift(dt, waterContactWeight);

        if (useAnchor)
            ApplyAnchor(dt);

        if (adjustHeight)
            ApplyHeight(dt);

        if (alignToWaterSurface && hasForwardSample && hasSideSample && waterContactWeight > 0.001f)
            ApplySurfaceTilt(dt, waterContactWeight);
    }

    private void SampleWater()
    {
        Vector3 center = transform.position;
        center.y = waterSurface.transform.position.y;

        Vector3 forward = center + transform.forward * forwardSampleDistance;
        Vector3 side = center + transform.right * sideSampleDistance;

        hasCenterSample = Sample(center, ref centerParams, ref centerResult);
        hasForwardSample = Sample(forward, ref forwardParams, ref forwardResult);
        hasSideSample = Sample(side, ref sideParams, ref sideResult);
    }

    private void UpdateWaterContactWeight()
    {
        if (!onlyAffectWhenWaterReachesObject)
        {
            waterContactWeight = 1f;
            return;
        }

        float waterY = centerResult.projectedPositionWS.y;
        float objectY = transform.position.y + waterContactHeightOffset;
        float difference = waterY - objectY;

        waterContactWeight = Mathf.InverseLerp(
            -waterContactFadeDistance,
            0f,
            difference
        );
    }

    private void ApplyWaveSlopeDrift(float dt, float influence)
    {
        if (influence <= 0f)
            return;

        float centerY = centerResult.projectedPositionWS.y;
        float forwardY = forwardResult.projectedPositionWS.y;
        float sideY = sideResult.projectedPositionWS.y;

        float forwardSlope = (forwardY - centerY) / Mathf.Max(0.001f, forwardSampleDistance);
        float sideSlope = (sideY - centerY) / Mathf.Max(0.001f, sideSampleDistance);

        Vector3 push =
            -transform.forward * forwardSlope +
            -transform.right * sideSlope;

        push.y = 0f;

        waveVelocity += push * waveDriftStrength * influence * dt;
        waveVelocity = Vector3.ClampMagnitude(waveVelocity, waveDriftMaxSpeed);
        waveVelocity = Vector3.Lerp(waveVelocity, Vector3.zero, waveDriftDamping * dt);

        transform.position += waveVelocity * dt;
    }

    private void ApplyRandomDrift(float dt, float influence)
    {
        if (influence <= 0f)
            return;

        float t = (float)(GetTime() * randomDriftSpeed + randomDriftPhase);

        Vector3 direction = new Vector3(
            Mathf.PerlinNoise(t, 11.7f) * 2f - 1f,
            0f,
            Mathf.PerlinNoise(t, 26.3f) * 2f - 1f
        );

        if (direction.sqrMagnitude > 0.001f)
            direction.Normalize();

        randomVelocity += direction * randomDriftStrength * influence * dt;
        randomVelocity = Vector3.ClampMagnitude(randomVelocity, randomDriftMaxSpeed);
        randomVelocity = Vector3.Lerp(randomVelocity, Vector3.zero, randomDriftDamping * dt);

        transform.position += randomVelocity * dt;
    }

    private void ApplyAnchor(float dt)
    {
        Vector3 liveAnchorPosition = anchorPoint != null ? anchorPoint.position : anchorPosition;

        Vector3 currentFlat = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 anchorFlat = new Vector3(liveAnchorPosition.x, 0f, liveAnchorPosition.z);

        Vector3 fromAnchor = currentFlat - anchorFlat;
        float distance = fromAnchor.magnitude;

        if (distance > anchorRadius)
        {
            Vector3 pullDirection = -fromAnchor.normalized;
            anchorVelocity += pullDirection * anchorPullStrength * dt;
        }

        anchorVelocity = Vector3.ClampMagnitude(anchorVelocity, anchorMaxSpeed);
        anchorVelocity = Vector3.Lerp(anchorVelocity, Vector3.zero, anchorDamping * dt);

        transform.position += anchorVelocity * dt;
    }

    private void ApplyHeight(float dt)
    {
        float targetY = centerResult.projectedPositionWS.y + waterHeightOffset;

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, targetY, SmoothFactor(heightSmooth, dt));
        transform.position = pos;
    }

    private void ApplySurfaceTilt(float dt, float influence)
    {
        float centerY = centerResult.projectedPositionWS.y;
        float forwardY = forwardResult.projectedPositionWS.y;
        float sideY = sideResult.projectedPositionWS.y;

        float pitch = Mathf.Atan2(forwardY - centerY, forwardSampleDistance) * Mathf.Rad2Deg;
        float roll = -Mathf.Atan2(sideY - centerY, sideSampleDistance) * Mathf.Rad2Deg;

        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle) * influence;
        roll = Mathf.Clamp(roll, -maxRollAngle, maxRollAngle) * influence;

        float currentYaw = transform.eulerAngles.y;

        Quaternion targetRotation =
            Quaternion.Euler(0f, currentYaw, 0f) *
            Quaternion.Euler(rotationOffset) *
            Quaternion.Euler(pitch, 0f, roll);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            SmoothFactor(rotationSmooth, dt)
        );
    }

    private bool Sample(
        Vector3 target,
        ref WaterSearchParameters parameters,
        ref WaterSearchResult result)
    {
        parameters.startPositionWS = result.candidateLocationWS;
        parameters.targetPositionWS = target;
        parameters.error = 0.01f;
        parameters.maxIterations = 8;

        return waterSurface.ProjectPointOnWaterSurface(parameters, out result);
    }

    private float SmoothFactor(float smooth, float dt)
    {
        if (smooth <= 0f)
            return 1f;

        return 1f - Mathf.Exp(-smooth * dt);
    }

    private float GetDeltaTime()
    {
        return Application.isPlaying ? Time.deltaTime : 1f / 60f;
    }

    private double GetTime()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return EditorApplication.timeSinceStartup;
#endif

        return Time.time;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos)
            return;

        Vector3 liveAnchorPosition = anchorPoint != null ? anchorPoint.position : anchorPosition;

        Vector3 center = transform.position;
        Vector3 forward = center + transform.forward * forwardSampleDistance;
        Vector3 side = center + transform.right * sideSampleDistance;

        Gizmos.DrawWireSphere(center, 0.12f);
        Gizmos.DrawWireSphere(forward, 0.12f);
        Gizmos.DrawWireSphere(side, 0.12f);

        Gizmos.DrawLine(center, forward);
        Gizmos.DrawLine(center, side);

        Gizmos.DrawWireSphere(liveAnchorPosition, anchorRadius);
        Gizmos.DrawLine(liveAnchorPosition, transform.position);
    }
}