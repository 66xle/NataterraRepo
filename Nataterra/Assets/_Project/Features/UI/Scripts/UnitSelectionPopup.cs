using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionPopup : MonoBehaviour
{
    [SerializeField] Image SelectedImage;
    [SerializeField] Image TextureImage;
    [SerializeField] TMP_Text UnitNameText;
    [SerializeField] TMP_Text MovementText;
    [SerializeField] TMP_Text SelectedUnitText;
    [SerializeField] TMP_Text RemainingUnitText;

    UnitSelectionIcon _icon;



    public void SetData(UnitSelectionIcon icon, int movement, Sprite sprite)
    {
        _icon = icon;

        string unitType = icon.GetUnitType().ToString();

        int remainingUnits = icon.DictMovementUnits.ContainsKey(movement) ? icon.DictMovementUnits[movement].Count : 0;
        int selectedUnits  = icon.DictSelectedUnits.ContainsKey(movement) ? icon.DictSelectedUnits[movement].Count : 0;

        TextureImage.sprite = sprite;
        UnitNameText.text = unitType;
        MovementText.text = $"Movement: {movement.ToString()}";
        SelectedUnitText.text = $"Selected: {selectedUnits.ToString()}";
        RemainingUnitText.text = $"Remaining: {remainingUnits.ToString()}";

        SelectedImage.color = selectedUnits > 0 ? Color.green : Color.white;
    }
}
