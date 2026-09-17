using UnityEngine;

[CreateAssetMenu(fileName = "FlockSetting", menuName = "FlockSetting" )]
public class FlockSetting : ScriptableObject
{
    [Header("Movement")]
    public float MaxSpeed = 7f;
    public float AccelerationForce = 18f;
    public float RotationSharpness = 10f;

    [Header("Behaviour Distances")]
    public float SeparationDistance = 1.5f;
    public float AlignmentDistance = 3.5f;
    public float CohesionDistance = 4.5f;

    [Header("Behaviour Weights")]
    public float SeparationWeight = 1.6f;
    public float AlignmentWeight = 1f;
    public float CohesionWeight = 1.2f;
    public float EdgeWeight = 3f;

    [Header("Edge Avoidance")]
    public float EdgeLookAhead = 3.5f;
}
