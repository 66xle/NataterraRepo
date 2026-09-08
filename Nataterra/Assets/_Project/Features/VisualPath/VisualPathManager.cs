using DrawXXL;
using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VisualPath;

public class VisualPathManager : MonoBehaviour
{
    [Header("Path Settings")]
    public PathPriorityMode PathPriority;
    public EdgeAvoidanceMode EdgeAvoidance;
    [Range(0, 1)] public float EdgeAvoidanceAmount = 0.35f;
    [Range(0, 1)] public float EdgeMargin = 0.25f;
    public int PortalRelaxationPasses = 4;
    public float StraightAngleTolerance = 6f;
    public int SmoothingSamples = 8;
    [Range(0, 1)] public float SmoothingRatio = 0.2f;

    [Header("Road Exit")]
    public int RoadExitSamplesPerSegment = 10;
    public float RoadExitAngleRange = 30f;

    [Header("NavMesh")]
    public bool UseNavMesh = false;
    public NavMeshData NavMeshData;
    public int NavMeshAreaMask = NavMesh.AllAreas;
    public float NavMeshSampleDistance = 0.5f;
    public float NavMeshHeightTolerance = 1f;
    public float NavMeshValidationSampleDistance = 0.5f;
    public float NavMeshEdgeAvoidanceDistance = 0.5f;
    public int NavMeshEdgeAvoidanceIterations = 2;
    [Range(0, 1)] public float NavMeshEdgeAvoidanceStrength = 1f;
    public bool ValidateSmoothedPath = true;

    [Header("Other")]
    public bool UpdatePath = false;
    public List<Transform> RoadPoints;
    [SerializeReference, SR] public List<RoadWrapper> ListOfRoads; 


    VisualPathGenerator _generator;
    VisualPathRequest _request;

    LineRenderer _lineRenderer;
    List<Vector3> _path = new();

    List<VisualRoad> _roads = new();


    public void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        if (UpdatePath && _path.Count > 0)
        {
            GeneratePath();
            DisplayPath();
        }
    }

    public void CreateRequest(List<VisualHex> corridor, Vector3 start, Vector3 destination, List<VisualRoad> roads)
    {
        _roads = roads;
        _request = new VisualPathRequest(corridor, start, destination, roads);
    }

    public List<Vector3> GeneratePath()
    {
        VisualPathSettings settings = new VisualPathSettings
        {
            PathPriority = PathPriority,
            EdgeAvoidanceMode = EdgeAvoidance,
            EdgeAvoidanceAmount = EdgeAvoidanceAmount,
            EdgeMargin = EdgeMargin,
            PortalRelaxationPasses = PortalRelaxationPasses,

            RoadExitSamplesPerSegment = RoadExitSamplesPerSegment,
            StraightAngleTolerance = StraightAngleTolerance,
            SmoothingSamples = SmoothingSamples,
            SmoothingRatio = SmoothingRatio,
            RoadExitAngleRange = RoadExitAngleRange,

            UseNavMesh = UseNavMesh,
            NavMeshData = NavMeshData,
            NavMeshAreaMask = NavMeshAreaMask,
            NavMeshSampleDistance = NavMeshSampleDistance,
            NavMeshHeightTolerance = NavMeshHeightTolerance,
            NavMeshValidationSampleDistance = NavMeshValidationSampleDistance,
            NavMeshEdgeAvoidanceDistance = NavMeshEdgeAvoidanceDistance,
            NavMeshEdgeAvoidanceIterations = NavMeshEdgeAvoidanceIterations,
            NavMeshEdgeAvoidanceStrength = NavMeshEdgeAvoidanceStrength,
            ValidateSmoothedPath = ValidateSmoothedPath
        };

        _generator = new VisualPathGenerator(settings, NavMeshData);

        VisualPathResult result = _generator.Generate(_request);

        if (!result.Success)
        {
            Debug.LogError(result.Error);
            return null;
        }

        _path = new List<Vector3>(result.FinalPath);

        return _path;
    }

    public void DisplayPath()
    {
        _lineRenderer.positionCount = _path.Count;

        for (int i = 0; i < _path.Count; i++)
        {
            _lineRenderer.SetPosition(i, _path[i]);
        }
    }

    private void OnDrawGizmos()
    {
        DrawBasics.usedUnityLineDrawingMethod = DrawBasics.UsedUnityLineDrawingMethod.gizmoLines;

        foreach (RoadWrapper road in ListOfRoads)
        {
            for (int i = 0; i < road.Points.Count - 1; i++)
            {
                DrawBasics.Line(road.Points[i].position, road.Points[i + 1].position);
            }
        }
    }
}
