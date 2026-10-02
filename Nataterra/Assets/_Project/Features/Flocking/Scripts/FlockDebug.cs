using CodiceApp;
using DrawXXL;
using System.Collections.Generic;
using UnityEngine;

public class FlockDebug : MonoBehaviour
{
    public static FlockDebug Instance { get; protected set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }

    List<Vector3> Path = new();
    List<FlockAgent> Agents = new();

    private void Start()
    {
        DrawBasics.coneLength_interpretation_forStraightVectors = DrawBasics.LengthInterpretation.absoluteUnits;
    }

    void DrawPath()
    {
        for (int i = 1; i < Path.Count; i++) 
        {
            DrawBasics.Line(Path[i - 1], Path[i], Color.purple, 0.1f);
        }
    }

    void RemovePath()
    {
        if (Path.Count == 0) return;

        foreach (FlockAgent agent in Agents)
        {
            if (agent.HasArrived) continue;
            return;
        }

        Path.Clear();
    }

    public void SetPath(List<Vector3> path)
    {
        Path = path;
    }

    public void SetAgents(List<FlockAgent> agents)
    {
        Agents = agents;
    }

    public void DrawVector(Vector3 position, Vector3 target, Color color)
    {
        DrawBasics.Vector(position, target, color, 0, "", 1f);
    }

    public void DrawLine(Vector3 position, Vector3 target, Color color)
    {
        DrawBasics.Line(position, target, color);
    }

    public void AddPoint(Vector3 point)
    {
        RemovePath();

        Path.Add(point);
    }

    private void OnDrawGizmos()
    {
        DrawBasics.usedUnityLineDrawingMethod = DrawBasics.UsedUnityLineDrawingMethod.gizmoLines;

        DrawPath();


        DrawBasics.usedUnityLineDrawingMethod = DrawBasics.UsedUnityLineDrawingMethod.debugLines;
    }
}
