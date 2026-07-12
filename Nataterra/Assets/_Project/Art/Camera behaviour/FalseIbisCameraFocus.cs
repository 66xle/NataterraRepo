using UnityEngine;

public class FalseIbisCameraFocus : MonoBehaviour
{
    [Header("Global Orbit Focus")]
    [SerializeField] private Transform worldCentreFocus;

    [Header("Selection Focus")]
    [SerializeField] private Transform selectionFocus;
    [SerializeField] private Vector3 selectionFocusOffset = new Vector3(0f, 0f, 0f);
    [SerializeField] private bool clearSelectionFocusInGlobalOrbit = true;

    public Vector3 CurrentFocusPoint
    {
        get
        {
            if (selectionFocus != null)
                return selectionFocus.position + selectionFocusOffset;

            if (worldCentreFocus != null)
                return worldCentreFocus.position;

            return transform.position;
        }
    }

    public bool HasSelectionFocus => selectionFocus != null;

    public void Tick(FalseIbisCameraMode mode)
    {
        if (clearSelectionFocusInGlobalOrbit && mode == FalseIbisCameraMode.GlobalOrbit)
            ClearSelectionFocus();
    }

    public void SetWorldCentreFocus(Transform target)
    {
        worldCentreFocus = target;
    }

    public void SetSelectionFocus(Transform target)
    {
        selectionFocus = target;
    }

    public void ClearSelectionFocus()
    {
        selectionFocus = null;
    }
}