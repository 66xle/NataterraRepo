using PurrNet;
using TMPro;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    MapStateMachine MapCtx;

    public TMP_Text AvaliableText;

    private void Awake()
    {
        MapCtx = InstanceHandler.GetInstance<MapStateMachine>();
    }

    public void AddUnitToCart(int index)
    {
        MapCtx.OnUnitPurchase?.Invoke((UnitType)index);
    }
}
