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
        DrawPath();
        RemovePoint();
    }

    void DrawPath()
    {
        for (int i = 1; i < Path.Count; i++) 
        {
            DrawBasics.Line(Path[i - 1], Path[i], Color.blue);
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

    public void SetAgents(List<FlockAgent> agents)
    {
        Agents = agents;
    }

    public void DrawAgentTarget(Vector3 position, Vector3 target, Color color)
    {
        DrawBasics.Vector(position, target, Color.red, 0, "", 1f);
    }

    public void AddPoint(Vector3 point)
    {
        Path.Add(point);
    }
}
