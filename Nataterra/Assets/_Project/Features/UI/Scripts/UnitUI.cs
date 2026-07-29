using PurrNet;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Alchemy.Inspector;

public class UnitUI : MonoBehaviour
{
    MapStateMachine MapCtx;
    Button _button;

    UnitType _unitType;

    [Required] [SerializeField] Image Image;
    [Required] [SerializeField] TMP_Text NameText;
    [Required] [SerializeField] TMP_Text CostText;
    [Required] [SerializeField] TMP_Text AvaliableText;

    private void Awake()
    {
        MapCtx = InstanceHandler.GetInstance<MapStateMachine>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(AddUnitToCart);
    }

    public void SetData(UnitData data)
    {
        Image.sprite = data.Sprite;
        _unitType = data.UnitType;
        NameText.text = data.UnitType.ToString();
        SetUnitAvailiable(data.StartingAvailiableUnits);

        if (data.FoodCost > 0)
            CostText.text += $" {data.FoodCost} F";

        if (data.WoodCost > 0)
            CostText.text += $" {data.WoodCost} W";

        if (data.MetalCost > 0)
            CostText.text += $" {data.MetalCost} M";
    }

    public void SetUnitAvailiable(int amount)
    {
        AvaliableText.text = $"Avaliable: <color=green>{amount}</color>";
    }

    public void AddUnitToCart()
    {
        MapCtx.OnUnitPurchase?.Invoke(_unitType);
    }
}
