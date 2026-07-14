using PurrNet;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    MapStateMachine MapCtx;

    private void Start()
    {
        MapCtx = InstanceHandler.GetInstance<MapStateMachine>();
    }

    public void AddUnitToCart(int index)
    {
        MapCtx.OnUnitPurchase?.Invoke((UnitType)index);
    }

}
