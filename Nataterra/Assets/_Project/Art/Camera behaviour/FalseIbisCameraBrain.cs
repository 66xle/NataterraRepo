using UnityEngine;

[RequireComponent(typeof(FalseIbisCameraZoom))]
[RequireComponent(typeof(FalseIbisTerrainFollower))]
[RequireComponent(typeof(FalseIbisCameraRigSolver))]
[RequireComponent(typeof(FalseIbisCameraLens))]
public class FalseIbisCameraBrain : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Transform startingPosition;

    private FalseIbisCameraZoom zoom;
    private FalseIbisTerrainFollower terrainFollower;
    private FalseIbisCameraRigSolver rigSolver;
    private FalseIbisCameraLens lens;

    private void Awake()
    {
        zoom = GetComponent<FalseIbisCameraZoom>();
        terrainFollower = GetComponent<FalseIbisTerrainFollower>();
        rigSolver = GetComponent<FalseIbisCameraRigSolver>();
        lens = GetComponent<FalseIbisCameraLens>();

        if (targetCamera == null)
            targetCamera = Camera.main;

        if (startingPosition != null)
        {
            transform.position = startingPosition.position;
            transform.rotation = startingPosition.rotation;
        }

        terrainFollower.Initialize(transform);
    }

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;

        zoom.Tick(deltaTime);

        terrainFollower.Tick(transform, deltaTime);

        CameraRigPose pose = rigSolver.Solve(transform, zoom.CurrentZoom);

        if (targetCamera != null)
        {
            targetCamera.transform.position = pose.Position;
            targetCamera.transform.rotation = pose.Rotation;
        }

        lens.Apply(targetCamera, zoom.CurrentZoom, deltaTime);
    }
}