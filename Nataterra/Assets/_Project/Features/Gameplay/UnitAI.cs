using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAI : MonoBehaviour
{
    public float MoveDuration = 3f;
    public float StoppingDistance = 1f;

    public float SampleDistance = 1f;

    List<Vector3> _currentPath = new();
    NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = StoppingDistance;
    }

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (_currentPath.Count == 0) return;

        if (_agent.pathPending) return;

        if (_agent.remainingDistance > _agent.stoppingDistance) return;


        _currentPath.RemoveAt(0);

        if (_currentPath.Count == 0) return;


        _agent.SetDestination(_currentPath[0]);
    }

    public void SetPosition(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, SampleDistance, -1))
        {
            Debug.LogError("UnitAI: SetPosition: Failed to sample Unit Spawn Position");
            return;
        }

        _agent.Warp(hit.position);
    }

    public void SetPath(List<Vector3> path)
    {
        _currentPath = new(path);
    }
}
