using System.Collections.Generic;
using UnityEngine;
using VisualPath;

public class VisualPathManager : MonoBehaviour
{
    public List<VisualRoad> roads = new();

    private VisualPathGenerator _generator;

    public List<Vector3> Generate(List<VisualHex> corridor, Vector3 start, Vector3 destination)
    {
        VisualPathSettings settings = new VisualPathSettings
        {
            PathPriority = PathPriorityMode.OptimizedPathFirst,
            EdgeAvoidanceMode = EdgeAvoidanceMode.PortalCenterBias,
            EdgeAvoidanceAmount = 0.35f,
            EdgeMargin = 0.25f,
            PortalRelaxationPasses = 4,
            StraightAngleTolerance = 6f,
            SmoothingRatio = 0.2f
        };

        _generator = new VisualPathGenerator(settings);

        VisualPathRequest request = new VisualPathRequest(corridor, start, destination, roads);

        VisualPathResult result = _generator.Generate(request);

        if (!result.IsValid)
        {
            Debug.LogError(result.Error);
            return null;
        }

        List<Vector3> points = new List<Vector3>(result.FinalPoints);
        return points;
    }
}
