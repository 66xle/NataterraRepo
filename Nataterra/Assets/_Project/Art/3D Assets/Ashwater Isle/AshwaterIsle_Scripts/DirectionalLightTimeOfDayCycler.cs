using UnityEngine;

[ExecuteAlways]
public class DirectionalLightTimeOfDayCycler : MonoBehaviour
{
    [System.Serializable]
    public class LightTimeKey
    {
        public string name = "Time Key";
        public Vector3 rotation;
        [Min(0.1f)] public float durationToNext = 5f;
    }

    [Header("Time of Day Keys")]
    public LightTimeKey[] lightKeys = new LightTimeKey[3]
    {
        new LightTimeKey { name = "Morning", rotation = new Vector3(25f, -45f, 0f), durationToNext = 6f },
        new LightTimeKey { name = "Noon", rotation = new Vector3(65f, 0f, 0f), durationToNext = 6f },
        new LightTimeKey { name = "Evening", rotation = new Vector3(20f, 45f, 0f), durationToNext = 6f }
    };

    [Header("Playback")]
    [Range(0f, 10f)]
    public float timeSpeed = 1f;

    public bool playInEditMode = false;
    public bool smoothBlend = true;

    [Header("Debug")]
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private float segmentTimer = 0f;

    private void Update()
    {
        if (!Application.isPlaying && !playInEditMode)
            return;

        if (lightKeys == null || lightKeys.Length < 2)
            return;

        float deltaTime = Application.isPlaying ? Time.deltaTime : Time.unscaledDeltaTime;

        LightTimeKey currentKey = lightKeys[currentIndex];
        int nextIndex = (currentIndex + 1) % lightKeys.Length;
        LightTimeKey nextKey = lightKeys[nextIndex];

        float duration = Mathf.Max(0.1f, currentKey.durationToNext);
        segmentTimer += deltaTime * timeSpeed;

        float t = Mathf.Clamp01(segmentTimer / duration);

        if (smoothBlend)
            t = Mathf.SmoothStep(0f, 1f, t);

        Quaternion fromRotation = Quaternion.Euler(currentKey.rotation);
        Quaternion toRotation = Quaternion.Euler(nextKey.rotation);

        transform.rotation = Quaternion.Slerp(fromRotation, toRotation, t);

        if (segmentTimer >= duration)
        {
            segmentTimer = 0f;
            currentIndex = nextIndex;
        }
    }

    [ContextMenu("Capture Current Rotation To Current Key")]
    private void CaptureCurrentRotation()
    {
        if (lightKeys == null || lightKeys.Length == 0)
            return;

        currentIndex = Mathf.Clamp(currentIndex, 0, lightKeys.Length - 1);
        lightKeys[currentIndex].rotation = transform.eulerAngles;
    }

    [ContextMenu("Apply Current Key Rotation")]
    private void ApplyCurrentKey()
    {
        if (lightKeys == null || lightKeys.Length == 0)
            return;

        currentIndex = Mathf.Clamp(currentIndex, 0, lightKeys.Length - 1);
        transform.rotation = Quaternion.Euler(lightKeys[currentIndex].rotation);
    }

    public void ResetCycle()
    {
        currentIndex = 0;
        segmentTimer = 0f;

        if (lightKeys != null && lightKeys.Length > 0)
            transform.rotation = Quaternion.Euler(lightKeys[0].rotation);
    }
}