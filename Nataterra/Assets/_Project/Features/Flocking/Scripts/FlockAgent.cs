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
    Vector3 _target;

    List<FlockAgent> _listOfNeighbours = new();
    List<Vector3> _path = new();
    int _currentSegmentIndex;
    Vector3 _prevPoint;


    float _separationDistanceSqr;
    float _alignmentDistanceSqr;
    float _cohesionDistanceSqr;

    bool _hasArrived = true;

    public Vector3 Velocity => _agent ? _agent.velocity : Vector3.zero;

    // Debug
    public bool HasArrived => _hasArrived;


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
        UpdateCurrentSegment();
        PathFollowing();
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
            ComputeCohesion() * FS.CohesionWeight;

        Vector3 edgeAvoid = ComputeEdgeFollowing();

        if (edgeAvoid == Vector3.zero)
            steering += ComputeSeek() * FS.DestinationWeight;
        else
            steering += edgeAvoid;


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

    Vector3 GetPathPosition(Vector3 point, int segmentIndex)
    {
        Vector3 destination = _path[segmentIndex + 1];
        Vector3 current = _path[segmentIndex];

        Vector3 segment = destination - current;
        float t = Vector3.Dot(point - current, segment) / segment.sqrMagnitude;

        t = Mathf.Clamp01(t);

        Vector3 closestPoint = current + segment * t;
        return closestPoint;
    }

    bool IsInSegment(Vector3 position, int segmentIndex, out float progress)
    {
        Vector3 destination = _path[segmentIndex + 1];
        Vector3 current = _path[segmentIndex];

        Vector3 segment = destination - current;
        float t = Vector3.Dot(position - current, segment) / segment.sqrMagnitude;

        t = Mathf.Clamp01(t);

        progress = t;

        return t > 0 && t < 1f;
    }
    

    Vector3 ComputeSeek()
    {
        Vector3 desired_velocity = (_target - transform.position).normalized * FS.MaxSpeed;
        Vector3 steering = desired_velocity - _agent.velocity;
        return steering;
    }

    Vector3 ComputeEdgeFollowing()
    {
        if (!NavMesh.FindClosestEdge(transform.position, out NavMeshHit hit, -1))
            return Vector3.zero;


        Vector3 edgePos = hit.position;

        float distance = Vector3.Distance(transform.position, new Vector3(edgePos.x, transform.position.y, edgePos.z));

        if (distance > FS.EdgeDistance)
            return Vector3.zero;


        if (distance > _agent.radius)
        {
            NavMesh.SamplePosition(new Vector3(transform.position.x, edgePos.y, transform.position.z), out NavMeshHit surfaceHit, 0.3f, NavMesh.AllAreas);
            if (!NavMesh.Raycast(surfaceHit.position, _target, out NavMeshHit raycastHit, NavMesh.AllAreas))
                return Vector3.zero;
        }

        // Your existing method gives us the direction back onto the NavMesh.
        Vector3 edgeNormal = GetEdgeDirection(edgePos).normalized;

        if (edgeNormal == Vector3.zero)
            return Vector3.zero;


        // The wall has two possible directions we can travel along.
        Vector3 tangentA = Vector3.Cross(Vector3.up, edgeNormal).normalized;
        Vector3 tangentB = -tangentA;

        // Choose the tangent that generally takes us toward the path look-ahead.
        Vector3 toLookAhead = _target - transform.position;
        toLookAhead.y = 0f;

        float a = Vector3.Dot(tangentA, toLookAhead);
        float b = Vector3.Dot(tangentB, toLookAhead);

        Vector3 wallDirection = a > b ? tangentA : tangentB;

        // Keep the agent slightly inside the NavMesh.
        float distanceError = FS.EdgeDistance - distance;
        Vector3 stayAwayFromEdge = edgeNormal * distanceError;

        // Wall-following desired velocity.
        Vector3 desiredVelocity = wallDirection * FS.MaxSpeed + stayAwayFromEdge * FS.EdgeNormalWeight;

        // Convert desired velocity to steering.
        Vector3 steering = desiredVelocity - _agent.velocity;

        DrawBasics.Line(transform.position, edgePos, Color.yellow, 0, distance.ToString());
        FlockDebug.Instance.DrawVector(transform.position, transform.position + edgeNormal * 2f, Color.magenta);
        FlockDebug.Instance.DrawVector(transform.position, transform.position + wallDirection * 2f, Color.cyan);

        return steering;
    }

    Vector3 GetEdgeDirection(Vector3 position)
    {
        Vector3 edgeDirection = Vector3.zero;
        float length = 0.5f;

        bool up = NavMesh.SamplePosition(position + Vector3.forward * 0.3f, out _, length, NavMesh.AllAreas);
        bool left = NavMesh.SamplePosition(position + Vector3.left * 0.3f, out _, length, NavMesh.AllAreas);
        bool right = NavMesh.SamplePosition(position + Vector3.right * 0.3f, out _, length, NavMesh.AllAreas);
        bool down = NavMesh.SamplePosition(position + Vector3.back * 0.3f, out _, length, NavMesh.AllAreas);
        
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


    Vector3 GetLookAheadPosition(Vector3 currentPos, int segmentIndex, float lookAheadDistance)
    {
        Vector3 pos = currentPos;

        while (segmentIndex < _path.Count - 1)
        {
            Vector3 segmentStart = _path[segmentIndex];
            Vector3 segmentEnd = _path[segmentIndex + 1];
            segmentStart.y = currentPos.y;
            segmentEnd.y = currentPos.y;

            Vector3 pathDir = (segmentEnd - segmentStart).normalized;

            float distanceToEnd = Mathf.Max(0f, Vector3.Dot(segmentEnd - pos, pathDir));

            // Look-ahead is somewhere on this segment.
            if (distanceToEnd >= lookAheadDistance)
            {
                return pos + pathDir * lookAheadDistance;
            }

            // Look-ahead crosses this segment.
            lookAheadDistance -= distanceToEnd;
            pos = segmentEnd;
            segmentIndex++;
        }

        // We've reached the end of the path.
        return _destination;
    }

    void UpdateCurrentSegment()
    {
        if (_currentSegmentIndex >= _path.Count - 2) return;

        Vector3 waypoint = _path[_currentSegmentIndex + 1];
        Vector3 segmentDir = (_path[_currentSegmentIndex + 1] - _path[_currentSegmentIndex]).normalized;

        
        Vector3 currentPathPos = GetPathPosition(transform.position, _currentSegmentIndex);

        DrawBasics.PointTag(currentPathPos, "Player Position");

        float distanceToWaypoint = Vector3.Distance(currentPathPos, waypoint);

        if (IsInSegment(transform.position, _currentSegmentIndex, out float progress))
        {
            if (distanceToWaypoint < FS.ArrivalDistance && progress > 0.5f)
            {
                _currentSegmentIndex++;
            }
        }

        DrawBasics.PointTag(transform.position, _currentSegmentIndex.ToString(), Color.white, 0, 3f);
    }

    void PathFollowing()
    {
        Vector3 currentPathPos = GetPathPosition(transform.position, _currentSegmentIndex);
        Vector3 aheadPathPos = GetLookAheadPosition(currentPathPos, _currentSegmentIndex, FS.ArrivalDistance);

        FlockDebug.Instance.DrawVector(transform.position, aheadPathPos, Color.blue);

        _target = aheadPathPos;
        _agent.SetDestination(aheadPathPos);
    }

    void ArriveAtDestination()
    {
        float distance = Vector3.Distance(transform.position, _destination);

        if (distance > FS.ArrivalDistance) return;

        SetArrival();
    }

    public void SetArrival()
    {
        _hasArrived = true;
        _agent.ResetPath();
        _agent.isStopped = true;

        Debug.Log("Stop");
    }

    public void SetPath(List<Vector3> path)
    {
        _path.Clear();
        _path = path;

        _currentSegmentIndex = 0;

        _destination = _path[^1];
        _hasArrived = false;
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


    public void AddDestination(Vector3 target)
    {
        if (_hasArrived)
        {
            _currentSegmentIndex = 0;
            _path.Clear();
            _path.Add(transform.position);
        }

        _path.Add(target);
        _destination = target;
        _hasArrived = false;
    }

    public void SetNeighbours(List<FlockAgent> neighbours)
    {
        _listOfNeighbours = neighbours;
    }
}
