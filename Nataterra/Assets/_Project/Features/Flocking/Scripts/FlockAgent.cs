using DrawXXL;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static Codice.Client.Common.WebApi.WebApiEndpoints;
using static UnityEngine.GraphicsBuffer;

public class FlockAgent : MonoBehaviour
{
    [InspectorName("Flock Setting")] public FlockSetting FS;

    NavMeshAgent _agent;
    Vector3 _destination;

    List<FlockAgent> _listOfNeighbours = new();
    List<Vector3> _path = new();
    Vector3 _prevPoint;


    float _separationDistanceSqr;
    float _alignmentDistanceSqr;
    float _cohesionDistanceSqr;

    bool _hasArrived = true;

    public Vector3 Velocity => _agent ? _agent.velocity : Vector3.zero;

    // Debug
    public Vector3 Destination => _destination;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _separationDistanceSqr = FS.SeparationDistance * FS.SeparationDistance;
        _alignmentDistanceSqr = FS.AlignmentDistance * FS.AlignmentDistance;
        _cohesionDistanceSqr = FS.CohesionDistance * FS.CohesionDistance;
    }

    private void Update()
    {
        if (_hasArrived) return;

        ArriveAtDestination();
        Flocking();
    }

    void Flocking()
    {
        _separationDistanceSqr = FS.SeparationDistance * FS.SeparationDistance;
        _alignmentDistanceSqr = FS.AlignmentDistance * FS.AlignmentDistance;
        _cohesionDistanceSqr = FS.CohesionDistance * FS.CohesionDistance;

        Vector3 steering =
            ComputeSeparation() * FS.SeparationWeight +
            ComputeAlignment() * FS.AlignmentWeight +
            ComputeCohesion() * FS.CohesionWeight +
            ComputeSeek() * FS.DestinationWeight +
            ComputeEdgeAvoidance();


        var acceleration = (steering).normalized * FS.AccelerationForce;
        var nextVelocity = _agent.velocity + acceleration * Time.deltaTime;

        Vector3 currentVelocity = Vector3.ClampMagnitude(nextVelocity, FS.MaxSpeed);
        FlockDebug.Instance.DrawVector(transform.position, transform.position + currentVelocity, Color.red);

        _agent.velocity = currentVelocity;


        if (_agent.velocity.sqrMagnitude > 0.01f)
        { 
            var lookDir = Vector3.ProjectOnPlane(_agent.velocity, Vector3.up);
            var targetRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, FS.RotationSharpness * Time.deltaTime);
        }
    }

    Vector3 GetPathPosition(Vector3 point)
    {
        Vector3 segment = _destination - _prevPoint;
        float t = Vector3.Dot(point - _prevPoint, segment) / segment.sqrMagnitude;
        t = Mathf.Clamp01(t);

        Vector3 closestPoint = _prevPoint + segment * t;
        return closestPoint;
    }

    Vector3 ComputeSeek()
    {
        Vector3 desired_velocity = (_destination - transform.position).normalized * FS.MaxSpeed;
        Vector3 steering = desired_velocity - _agent.velocity;
        return steering;
    }

    Vector3 ComputeEdgeAvoidance()
    {
        //Vector3 aheadPos = transform.position + _agent.velocity.normalized * FS.EdgeLookAhead;

        NavMeshHit hit;
        if (!NavMesh.FindClosestEdge(transform.position, out hit, -1))
            return Vector3.zero;

        Vector3 edgePos = hit.position;
        if (NavMesh.Raycast(transform.position, edgePos, out hit, -1))
            return Vector3.zero;

        FlockDebug.Instance.DrawLine(transform.position, edgePos, Color.yellow);


        float distance = Vector3.Distance(transform.position, new Vector3(edgePos.x, transform.position.y, edgePos.z));

        if (distance > FS.EdgeDistance)
            return Vector3.zero;

        

        Vector3 pathPos = GetPathPosition(edgePos);
        Vector3 toPathDir = (pathPos - edgePos).normalized * FS.ToPathWeight;


        Vector3 normalDir = GetEdgeDirection(edgePos);

        edgePos.y = transform.position.y;
        distance = Vector3.Distance(transform.position, edgePos);

        float magnitude = distance == 0 ? normalDir.magnitude : distance;

        // Stronger force when closer to edge
        float strength = FS.EdgeNormalWeight * Mathf.InverseLerp(FS.EdgeDynamicMaxStrength, 0f, normalDir.magnitude);
        normalDir = normalDir * strength;


        FlockDebug.Instance.DrawVector(transform.position, transform.position + toPathDir, Color.darkCyan);
        FlockDebug.Instance.DrawVector(transform.position, transform.position + normalDir, Color.mediumPurple);

        return (toPathDir) + (normalDir);
    }

    Vector3 GetEdgeDirection(Vector3 position)
    {
        Vector3 edgeDirection = Vector3.zero;
        float length = 0.5f;

        bool up = NavMesh.SamplePosition(position + Vector3.forward, out _, length, NavMesh.AllAreas);
        bool left = NavMesh.SamplePosition(position + Vector3.left, out _, length, NavMesh.AllAreas);
        bool right = NavMesh.SamplePosition(position + Vector3.right, out _, length, NavMesh.AllAreas);
        bool down = NavMesh.SamplePosition(position + Vector3.back, out _, length, NavMesh.AllAreas);
        
        if (!up)
            edgeDirection -= Vector3.forward;

        if (!left)
            edgeDirection -= Vector3.left;

        if (!right)
            edgeDirection -= Vector3.right;

        if (!down)
            edgeDirection -= Vector3.back;

        if (edgeDirection != Vector3.zero)
            return edgeDirection;

        bool upRight = NavMesh.SamplePosition(position + Vector3.forward + Vector3.right, out _, length, NavMesh.AllAreas);
        bool upLeft = NavMesh.SamplePosition(position + Vector3.forward + Vector3.left, out _, length, NavMesh.AllAreas);
        bool downRight = NavMesh.SamplePosition(position + Vector3.back + Vector3.right, out _, length, NavMesh.AllAreas);
        bool downLeft = NavMesh.SamplePosition(position + Vector3.back + Vector3.left, out _, length, NavMesh.AllAreas);

        if (!upRight)
            edgeDirection -= Vector3.forward + Vector3.right;

        if (!upLeft)
            edgeDirection -= Vector3.forward + Vector3.left;

        if (!downRight)
            edgeDirection -= Vector3.back + Vector3.right;

        if (!downLeft)
            edgeDirection -= Vector3.back + Vector3.left;

        return edgeDirection;
    }

    Vector3 ComputeSeparation()
    {
        var force = Vector3.zero;
        var count = 0;
        var position = transform.position;

        for (int i = 0; i < _listOfNeighbours.Count; i++)
        {
            var toOther = position - _listOfNeighbours[i].transform.position;
            var sqrDistance = toOther.sqrMagnitude;
            if (sqrDistance > _separationDistanceSqr || sqrDistance < 0.0001f) continue;

            force += toOther / sqrDistance;
            count++;
        }

        return count > 0 ? force / count : Vector3.zero;
    }

    Vector3 ComputeAlignment()
    {
        var averageVelocity = Vector3.zero;
        var count = 0;
        var position = transform.position;

        for (int i = 0; i < _listOfNeighbours.Count; i++)
        {
            var offset = _listOfNeighbours[i].transform.position - position;
            if (offset.sqrMagnitude > _alignmentDistanceSqr) continue;

            averageVelocity += _listOfNeighbours[i].Velocity;
            count++;
        }

        return count > 0 ? (averageVelocity / count).normalized : Vector3.zero;
    }

    Vector3 ComputeCohesion()
    {
        var center = Vector3.zero;
        var count = 0;
        var position = transform.position;

        for (int i = 0; i < _listOfNeighbours.Count; i++)
        {
            var otherPosition = _listOfNeighbours[i].transform.position;
            if ((otherPosition - position).sqrMagnitude > _cohesionDistanceSqr) continue;

            center += otherPosition;
            count++;
        }

        return count > 0 ? (center / count - position).normalized : Vector3.zero;
    }


    public void ConfigureSpeed(float speed) => FS.MaxSpeed = Mathf.Max(0.1f, speed);


    void ArriveAtDestination()
    {
        float distance = Vector3.Distance(transform.position, _destination);

        FlockDebug.Instance.DrawVector(transform.position, _destination, Color.blue);

        if (distance > FS.ArrivalDistance) return;

        FlockDebug.Instance.DrawVector(transform.position, _destination, Color.green);

        if (_path.Count > 0)
        {
            SetDestination(_path[0]);
        }
        else
        {
            SetArrival();
        }
    }


    public void SetArrival()
    {
        _hasArrived = true;
        _agent.ResetPath();

        Debug.Log("Stop");
    }

    public void SetPath(List<Vector3> path)
    {
        _path = path;

        _destination = _path[0];
        SetDestination(_path[1]);
    }

    public void SetPosition(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, -1))
        {
            Debug.LogError("FlockAgent: SetPosition: Failed to sample Unit Spawn Position");
            return;
        }

        _agent.Warp(hit.position);
    }

    public void SetDestination(Vector3 target)
    {
        _prevPoint = _destination;

        _hasArrived = false;
        _destination = target;

        _agent.SetDestination(target);

        _path.Remove(_prevPoint);
        _path.Remove(_destination);
    }

    public void AddDestination(Vector3 target)
    {
        _path.Add(target);

        if (_hasArrived)
            SetDestination(_path[0]);
    }

    public void SetNeighbours(List<FlockAgent> neighbours)
    {
        _listOfNeighbours = neighbours;
    }
}
