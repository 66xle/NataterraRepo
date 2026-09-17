using Alchemy.Inspector;
using DrawXXL;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

public class FlockingManager : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject Prefab;
    public Transform SpawnLocation;
    public int SpawnCount = 3;

    [Header("Movement")]
    [Min(0.1f)] public float BaseSpeed = 7f;
    public float ArrivalDistance = 3f;

    List<FlockAgent> _agents = new();

    Vector3 _destination;

    [Button, Group("Spawning")]
    public void SpawnAgent()
    {
        FlockAgent agent = Instantiate(Prefab, SpawnLocation).GetComponent<FlockAgent>();
        agent.ConfigureSpeed(BaseSpeed);

        _agents.Add(agent);

        SetNeighbours();
    }

    [Button, Group("Spawning")]
    public void RemoveAgent()
    {
        if (_agents.Count == 0) return;

        FlockAgent agent = _agents[0];
        _agents.Remove(agent);

        Destroy(agent.gameObject);

        SetNeighbours();
    }


    public void Start()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            SpawnAgent();
        }

        SetNeighbours();
    }

    private void Update()
    {
        //ArriveAtDestination();
    }


    void ArriveAtDestination()
    {
        Vector3 groupPos = GetGroupCenter();
        groupPos.y = transform.position.y;

        float distance = Vector3.Distance(_destination, groupPos);
        DrawBasics.Vector(groupPos, _destination, Color.red);
        DrawBasics.PointTag(groupPos, distance.ToString(), Color.white, 0, 5f);
        
        if (distance > ArrivalDistance) return;

        DrawBasics.Vector(groupPos, _destination, Color.green);

        foreach (FlockAgent agent in _agents)
        {
            agent.SetArrival();
        }

    }

    Vector3 GetGroupCenter()
    {
        Vector3 center = Vector3.zero;

        foreach (FlockAgent agent in _agents)
        {
            Vector3 position = agent.transform.position;
            center += position;
        }

        return center / _agents.Count;
    }

    public void AddDestination(Vector3 target)
    {
        FlockDebug.Instance.AddPoint(target);

        _destination = target;

        foreach (FlockAgent agent in _agents)
        {
            agent.AddDestination(target);
        }
    }

    public void SetNeighbours()
    {
        FlockDebug.Instance.SetAgents(_agents);

        foreach (FlockAgent agent in _agents)
        {
            List<FlockAgent> neighbours = new(_agents);
            neighbours.Remove(agent);

            agent.SetNeighbours(neighbours);
        }
    }
}
