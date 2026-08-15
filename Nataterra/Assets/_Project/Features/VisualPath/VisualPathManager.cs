using System.Collections.Generic;
using UnityEngine;
using VisualPath;
using DrawXXL;

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

    [Header("Other")]
    public bool UpdatePath = false;
    public List<Transform> RoadPoints;


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

    public void GeneratePath()
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
            RoadExitAngleRange = RoadExitAngleRange
        };

        _generator = new VisualPathGenerator(settings);

        VisualPathResult result = _generator.Generate(_request);

        if (!result.Success)
        {
            Debug.LogError(result.Error);
            return;
        }

        

        _path = new List<Vector3>(result.FinalPath);
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

        for (int i = 0; i < RoadPoints.Count - 1; i++)
        {
            DrawBasics.Line(RoadPoints[i].position, RoadPoints[i + 1].position);
        }
    }
}
