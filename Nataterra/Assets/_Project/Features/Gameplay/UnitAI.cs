using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class UnitAI : MonoBehaviour
{
    [Header("Movement")]
    public float MoveDuration = 3f;
    public float MinSpeed = 3f;
    public float MaxSpeed = 5f;
    public float StoppingDistance = 1f;

    [Header("Rotation")]
    public float RotationSpeed = 2f;

    [Header("Navmesh")]
    public float SampleDistance = 1f;

    

    List<Vector3> _currentPath = new();
    NavMeshAgent _agent;

    bool isMoving = false;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
    }

    void Update()
    {
        MoveAlongPath();
        Rotation();
    }

    void MoveAlongPath()
    {
        if (!isMoving) return;

        if (!NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 3f, -1)) return;

        float distance = Vector3.Distance(hit.position, _currentPath[0]);
        if (distance > StoppingDistance) return;

        _currentPath.RemoveAt(0);

        // No more waypoints.
        if (_currentPath.Count == 0)
        {
            Debug.Log("Finish Pathing");

            _agent.ResetPath();
            isMoving = false;
            return;
        }

        Debug.Log("Next Point");
        _agent.SetDestination(_currentPath[0]);
        isMoving = true;
    }

    void Rotation()
    {
        if (!isMoving) return;

        Vector3 direction = _currentPath[0] - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * RotationSpeed);
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
        _agent.ResetPath();

        _currentPath = new(path);
        _currentPath.RemoveAt(0); // Start position

        float distance = 0;
        for (int i = 1; i < _currentPath.Count; i++)
        {
            distance += Vector3.Distance(_currentPath[i - 1], _currentPath[i]);
        }

        float speed = distance / MoveDuration;
        _agent.speed = Mathf.Clamp(speed, MinSpeed, MaxSpeed);

        _agent.SetDestination(_currentPath[0]);
        isMoving = true;
    }
}
