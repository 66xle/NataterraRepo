using DrawXXL;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

public class FlockAgent : MonoBehaviour
{
    [InspectorName("Flock Setting")] public FlockSetting FS;

    NavMeshAgent _agent;
    Vector3 _destination;

    List<FlockAgent> _listOfNeighbours = new();
    List<Vector3> _path = new();


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
            ComputeCohesion() * FS.CohesionWeight; //+
                                                   //ComputeEdgeAvoidance() * FS.EdgeWeight;


        Vector3 targetDir = (_destination - transform.position) * FS.PathWeight;

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


    public void ConfigureSpeed(float speed) => FS.MaxSpeed = Mathf.Max(0.1f, speed);


    void ArriveAtDestination()
    {
        Vector3 agentPos = transform.position;

        float distance = Vector3.Distance(_destination, agentPos);

        FlockDebug.Instance.DrawAgentTarget(agentPos, _destination, Color.red);

        if (distance > FS.ArrivalDistance) return;

        FlockDebug.Instance.DrawAgentTarget(agentPos, _destination, Color.green);

        

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

    public void SetDestination(Vector3 target)
    {
        _path.RemoveAt(0);

        _hasArrived = false;
        _destination = target;

        _agent.SetDestination(target);
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
