using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlockAgent : MonoBehaviour
{
    [InspectorName("Flock Setting")] public FlockSetting FS;

    NavMeshAgent _agent;
    Vector3 _destination;

    List<FlockAgent> _listOfNeighbours = new();

    float _separationDistanceSqr;
    float _alignmentDistanceSqr;
    float _cohesionDistanceSqr;

    bool _hasArrived = true;

    public Vector3 Velocity => _agent ? _agent.velocity : Vector3.zero;


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

        _separationDistanceSqr = FS.SeparationDistance * FS.SeparationDistance;
        _alignmentDistanceSqr = FS.AlignmentDistance * FS.AlignmentDistance;
        _cohesionDistanceSqr = FS.CohesionDistance * FS.CohesionDistance;

        Vector3 steering =
            ComputeSeparation() * FS.SeparationWeight +
            ComputeAlignment() * FS.AlignmentWeight +
            ComputeCohesion() * FS.CohesionWeight; //+
                                                   //ComputeEdgeAvoidance() * FS.EdgeWeight;


        Vector3 targetDir = _destination - transform.position;

        var acceleration = (targetDir + steering).normalized * FS.AccelerationForce;
        var nextVelocity = _agent.velocity + acceleration * Time.deltaTime;

        _agent.velocity = Vector3.ClampMagnitude(nextVelocity, FS.MaxSpeed);

        if (_agent.velocity.sqrMagnitude > 0.01f)
        {
            var lookDir = Vector3.ProjectOnPlane(targetDir + steering, Vector3.up);
            var targetRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, FS.RotationSharpness * Time.deltaTime);
        }
    }

    Vector3 ComputeSeparation()
    {
        var force = Vector3.zero;
        var count = 0;
        var position = transform.position;

        for (int i = 0; i < _listOfNeighbours.Count; i++)
        {
            var toOther = position - _listOfNeighbours[i].transform.position;
            var sqrDistance = toOther.magnitude;
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

    Vector3 ComputeEdgeAvoidance()
    {
        var velocity = _agent.velocity;
        if (velocity.sqrMagnitude < 0.0001f) return Vector3.zero;

        var direction = velocity.normalized;
        var position = transform.position;
        var avoidance = Vector3.zero;

        if (!NavMesh.FindClosestEdge(position, out NavMeshHit hit, -1))
            return Vector3.zero;

        float distance = hit.distance;
        if (distance >= FS.EdgeLookAhead)
            return Vector3.zero;

        // Is the agent moving toward the edge?
        float towardEdge = Vector3.Dot(direction, -hit.normal);
        if (towardEdge <= 0f)
            return Vector3.zero;

        float strength = 1f - Mathf.Clamp01(distance / FS.EdgeLookAhead);


        return hit.normal * (strength * towardEdge); ;
    }

    public void ConfigureSpeed(float speed) => FS.MaxSpeed = Mathf.Max(0.1f, speed);

    public void SetArrival()
    {
        _hasArrived = true;
        _agent.ResetPath();

        Debug.Log("Stop");
    }

    public void SetDestination(Vector3 target)
    {
        _hasArrived = false;
        _destination = target;

        _agent.SetDestination(target);
    }

    public void SetNeighbours(List<FlockAgent> neighbours)
    {
        _listOfNeighbours = neighbours;
    }

}
