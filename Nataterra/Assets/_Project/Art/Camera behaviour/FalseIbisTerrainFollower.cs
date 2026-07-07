using UnityEngine;

public class FalseIbisTerrainFollower : MonoBehaviour
{
    [Header("Terrain Following")]
    [SerializeField] private bool followTerrainHeight = true;
    [SerializeField] private Terrain terrainToFollow;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float heightAboveGround = 0f;
    [SerializeField] private float terrainFollowSmoothing = 8f;

    private float currentGroundHeight;
    private bool initialized;

    public void Initialize(Transform rigTransform)
    {
        currentGroundHeight = rigTransform.position.y;
        initialized = true;
    }

    public void Tick(Transform rigTransform, float deltaTime)
    {
        if (!followTerrainHeight)
            return;

        if (!initialized)
            Initialize(rigTransform);

        float desiredGroundHeight = rigTransform.position.y;

        if (terrainToFollow != null)
        {
            desiredGroundHeight =
                terrainToFollow.SampleHeight(rigTransform.position)
                + terrainToFollow.transform.position.y;
        }
        else
        {
            Vector3 rayOrigin = rigTransform.position + Vector3.up * 200f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 500f, groundMask))
                desiredGroundHeight = hit.point.y;
        }

        currentGroundHeight = Mathf.Lerp(
            currentGroundHeight,
            desiredGroundHeight + heightAboveGround,
            1f - Mathf.Exp(-terrainFollowSmoothing * deltaTime)
        );

        Vector3 position = rigTransform.position;
        position.y = currentGroundHeight;
        rigTransform.position = position;
    }
}