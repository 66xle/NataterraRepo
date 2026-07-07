using UnityEngine;

[RequireComponent(typeof(FalseIbisCameraZoom))]
[RequireComponent(typeof(FalseIbisCameraModeResolver))]
[RequireComponent(typeof(FalseIbisCameraFocus))]
[RequireComponent(typeof(FalseIbisCameraRotation))]
[RequireComponent(typeof(FalseIbisCameraMovement))]
[RequireComponent(typeof(FalseIbisTerrainFollower))]
[RequireComponent(typeof(FalseIbisCameraRigSolver))]
[RequireComponent(typeof(FalseIbisCameraLens))]
public class FalseIbisCameraBrain : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Transform startingPosition;

    private FalseIbisCameraZoom zoom;
    private FalseIbisCameraModeResolver modeResolver;
    private FalseIbisCameraFocus focus;
    private FalseIbisCameraRotation rotation;
    private FalseIbisCameraMovement movement;
    private FalseIbisTerrainFollower terrainFollower;
    private FalseIbisCameraRigSolver rigSolver;
    private FalseIbisCameraLens lens;

    private void Awake()
    {
        zoom = GetComponent<FalseIbisCameraZoom>();
        modeResolver = GetComponent<FalseIbisCameraModeResolver>();
        focus = GetComponent<FalseIbisCameraFocus>();
        rotation = GetComponent<FalseIbisCameraRotation>();
        movement = GetComponent<FalseIbisCameraMovement>();
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

        rotation.Initialize(transform);
        movement.Initialize(transform);
        terrainFollower.Initialize(transform);
    }

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;

        zoom.Tick(deltaTime);

        modeResolver.Tick(zoom.CurrentZoom);

        focus.Tick(modeResolver.CurrentMode);

        rotation.Tick(
            transform,
            modeResolver.CurrentMode,
            deltaTime
        );

        movement.Tick(
            transform,
            modeResolver.CurrentMode,
            deltaTime
        );

        terrainFollower.Tick(
            transform,
            deltaTime
        );

        CameraRigPose pose = rigSolver.Solve(
            transform,
            zoom.CurrentZoom,
            modeResolver.CurrentMode,
            focus.CurrentFocusPoint,
            rotation.GlobalOrbitPitch,
            rotation.GlobalOrbitYaw,
            zoom.OrbitDistanceOffset,
            deltaTime
        );

        if (targetCamera != null)
        {
            targetCamera.transform.position = pose.Position;
            targetCamera.transform.rotation = pose.Rotation;
        }

        lens.Apply(
            targetCamera,
            zoom.CurrentZoom,
            deltaTime
        );
    }
}