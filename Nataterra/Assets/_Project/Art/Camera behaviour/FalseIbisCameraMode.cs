using UnityEngine;

public enum FalseIbisCameraMode
{
    GlobalOrbit,
    Strategy,
    Inspect,
    Drive
}

public class FalseIbisCameraModeResolver : MonoBehaviour
{
    [Header("Mode Thresholds")]
    [Range(0f, 1f)] [SerializeField] private float globalOrbitZoomThreshold = 0.9f;
    [Range(0f, 1f)] [SerializeField] private float strategyZoomThreshold = 0.72f;
    [Range(0f, 1f)] [SerializeField] private float driveZoomThreshold = 0.07f;

    public FalseIbisCameraMode CurrentMode { get; private set; }

    public void Tick(float zoom)
    {
        if (zoom >= globalOrbitZoomThreshold)
            CurrentMode = FalseIbisCameraMode.GlobalOrbit;
        else if (zoom >= strategyZoomThreshold)
            CurrentMode = FalseIbisCameraMode.Strategy;
        else if (zoom <= driveZoomThreshold)
            CurrentMode = FalseIbisCameraMode.Drive;
        else
            CurrentMode = FalseIbisCameraMode.Inspect;
    }
}