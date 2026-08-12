using System.Collections.Generic;
using UnityEngine;

public sealed class VisualPathResult
{
    public bool Success { get; }

    public string Error { get; }

    public IReadOnlyList<Vector3> RawPath { get; }

    public IReadOnlyList<Vector3> OptimizedPath { get; }

    public IReadOnlyList<Vector3> FinalPath { get; }

    public bool UsesRoad { get; }

    private VisualPathResult(
        bool success,
        string error,
        IReadOnlyList<Vector3> rawPath,
        IReadOnlyList<Vector3> optimizedPath,
        IReadOnlyList<Vector3> finalPath,
        bool usesRoad)
    {
        Success = success;
        Error = error;
        RawPath = rawPath;
        OptimizedPath = optimizedPath;
        FinalPath = finalPath;
        UsesRoad = usesRoad;
    }

    public static VisualPathResult Create(
        IReadOnlyList<Vector3> rawPath,
        IReadOnlyList<Vector3> optimizedPath,
        IReadOnlyList<Vector3> finalPath,
        bool usesRoad)
    {
        return new VisualPathResult(
            true,
            null,
            rawPath,
            optimizedPath,
            finalPath,
            usesRoad);
    }

    public static VisualPathResult Failed(
        string error)
    {
        return new VisualPathResult(
            false,
            error,
            null,
            null,
            null,
            false);
    }
}