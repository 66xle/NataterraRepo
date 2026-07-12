using UnityEngine;

[CreateAssetMenu(menuName = "Water/Water Wave Settings")]
public class WaterWaveSettings : ScriptableObject
{
    [Header("Primary Wave")]
    public Vector2 primaryDirection = new Vector2(1f, 0.25f);
    public float primaryScale = 0.35f;
    public float primarySpeed = 0.45f;
    public float primaryHeight = 0.15f;

    [Header("Secondary Wave")]
    public Vector2 secondaryDirection = new Vector2(-0.4f, 1f);
    public float secondaryScale = 0.6f;
    public float secondarySpeed = 0.25f;
    public float secondaryHeight = 0.06f;

    public float GetHeight(Vector3 worldPosition, float time)
    {
        Vector2 pos = new Vector2(worldPosition.x, worldPosition.z);

        Vector2 dirA = primaryDirection.normalized;
        Vector2 dirB = secondaryDirection.normalized;

        float waveA = Mathf.Sin(Vector2.Dot(pos, dirA) * primaryScale + time * primarySpeed) * primaryHeight;
        float waveB = Mathf.Sin(Vector2.Dot(pos, dirB) * secondaryScale + time * secondarySpeed) * secondaryHeight;

        return waveA + waveB;
    }
}