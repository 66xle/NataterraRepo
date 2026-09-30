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

    private void Update()
    {
        RemovePoint();
    }

    void DrawPath()
    {
        DrawBasics.usedUnityLineDrawingMethod = DrawBasics.UsedUnityLineDrawingMethod.debugLines;

        for (int i = 1; i < Path.Count; i++) 
        {
            DrawBasics.Line(Path[i - 1], Path[i], Color.purple);
        }
    }

    void RemovePoint()
    {
        if (Path.Count == 0) return;

        int lowestIndex = 10000;

        foreach (FlockAgent agent in Agents)
        {
            if (!Path.Contains(agent.Destination)) continue;

            int index = Path.IndexOf(agent.Destination);

            if (index < lowestIndex)
                lowestIndex = index;
        }

        if (lowestIndex > 0)
            Path.RemoveAt(0);
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
        Path.Add(point);
    }

    private void OnDrawGizmos()
    {
        DrawBasics.usedUnityLineDrawingMethod = DrawBasics.UsedUnityLineDrawingMethod.gizmoLines;
        DrawPath();
    }
}
