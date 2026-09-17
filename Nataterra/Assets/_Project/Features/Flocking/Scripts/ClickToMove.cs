using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public FlockingManager FlockingManager;
    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            if (!NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas)) return;

            FlockingManager.SetDestination(navHit.position);
        }
    }
}