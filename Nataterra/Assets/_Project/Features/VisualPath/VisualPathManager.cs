using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
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
    [Range(0, 1)] public float SmoothingRatio = 0.2f;

    [Header("Other")]
    public bool UpdatePath = false;
    List<VisualRoad> roads = new();


    VisualPathGenerator _generator;
    VisualPathRequest _request;

    LineRenderer _lineRenderer;
    List<Vector3> _path = new();



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

    public void CreateRequest(List<VisualHex> corridor, Vector3 start, Vector3 destination)
    {
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
            StraightAngleTolerance = StraightAngleTolerance,
            SmoothingRatio = SmoothingRatio
        };

        _generator = new VisualPathGenerator(settings);

        VisualPathResult result = _generator.Generate(_request);

        if (!result.IsValid)
        {
            Debug.LogError(result.Error);
            return;
        }

        _path = new List<Vector3>(result.FinalPoints);
    }

    public void DisplayPath()
    {
        _lineRenderer.positionCount = _path.Count;

        for (int i = 0; i < _path.Count; i++)
        {
            _lineRenderer.SetPosition(i, _path[i]);
        }
    }
}
