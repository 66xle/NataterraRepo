
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectionPopup : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image SelectedImage;
    [SerializeField] Image TextureImage;
    [SerializeField] TMP_Text UnitNameText;
    [SerializeField] TMP_Text MovementText;
    [SerializeField] TMP_Text SelectedUnitText;
    [SerializeField] TMP_Text RemainingUnitText;

    UnitSelectionIcon _icon;
    int _movement;


    public void SetData(UnitSelectionIcon icon, int movement, Sprite sprite)
    {
        _icon = icon;
        _movement = movement;

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

    public void IconStateChange(int selected, int remaining)
    {
        SelectedImage.color = selected > 0 ? Color.green : Color.white;
        SelectedUnitText.text = $"Selected: {selected.ToString()}";
        RemainingUnitText.text = $"Remaining: {remaining.ToString()}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _icon.SelectIcon(this, _movement);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            _icon.DeselectIcon(this, _movement);
        }
    }
}
