using PurrNet;
using System.Collections.Generic;
using System.Linq;
using TGS;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UnitSelectionIcon : MonoBehaviour, IPointerClickHandler
{
    MapStateMachine MapCtx;

    [SerializeField] Material XrayMaterial;
    [SerializeField] Image SelectedImage;
    [SerializeField] Image TextureImage;
    [SerializeField] TMP_Text RemainingUnitText;
    [SerializeField] TMP_Text SelectedUnitText;

    UnitType _unitType;
    Dictionary<int, List<Unit>> _dictMovementUnits;
    Dictionary<int, List<Unit>> _dictSelectedUnits = new();

    List<int> _listMovements = new();

    private void Start()
    {
        MapCtx = InstanceHandler.GetInstance<MapStateMachine>();
    }
    void OnDestroy()
    {
        if (XrayMaterial != null)
            DestroyImmediate(XrayMaterial);
    }

    public void SelectIcon(bool value)
    {
        SelectedImage.gameObject.SetActive(value);
    }

    public void SetData(Texture2D texture, UnitType type, Dictionary<int, List<Unit>> dictMovementUnits)
    {
        Material newMat = new Material(XrayMaterial);
        newMat.SetTexture("_MainTex", texture);

        TextureImage.material = newMat;
        _unitType = type;
        _dictMovementUnits = dictMovementUnits;

        RemainingUnitText.text = CheckDictSize(_dictMovementUnits).ToString();
        SelectedUnitText.text = "0";
        SelectIcon(false);

        StoreMovement();
    }

    void StoreMovement()
    {
        _listMovements.Clear();

        foreach (int movement in _dictMovementUnits.Keys)
        {
            _listMovements.Add(movement);
        }

        _listMovements.Sort();
    }

    int CheckDictSize(Dictionary<int, List<Unit>> dict)
    {
        int count = 0;

        foreach (int movement in dict.Keys)
        {
            count += dict[movement].Count;
        }

        return count;
    }

    Unit SelectUnit()
    {
        foreach (int movement in _listMovements)
        {
            if (_dictMovementUnits[movement].Count == 0)
                continue;

            Unit selectedUnit = _dictMovementUnits[movement][0];
            _dictMovementUnits[movement].Remove(selectedUnit);

            if (_dictSelectedUnits.ContainsKey(movement))
            {
                _dictSelectedUnits[movement].Add(selectedUnit);
            }
            else
            {
                _dictSelectedUnits[movement] = new List<Unit> { selectedUnit };
            }

            return selectedUnit;
        }

        return null;
    }

    Unit DeselectUnit()
    {
        for (int i = _listMovements.Count - 1; i >= 0; i--)
        {
            int movement = _listMovements[i];
            if (_dictSelectedUnits[movement].Count == 0)
                continue;

            Unit unit = _dictSelectedUnits[movement][0];
            _dictSelectedUnits[movement].Remove(unit);
            _dictMovementUnits[movement].Add(unit);

            return unit;
        }

        return null;
    }

    void IconStateChange()
    {
        int count = CheckDictSize(_dictSelectedUnits);
        if (count > 0)
        {
            SelectIcon(true);
        }
        else
        {
            SelectIcon(false);
        }

        RemainingUnitText.text = CheckDictSize(_dictMovementUnits).ToString();
        SelectedUnitText.text = count.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Unit unit = SelectUnit();
            if (unit == null)
                return;

            IconStateChange();

            MapCtx.OnUnitSelect?.Invoke(unit);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Unit unit = DeselectUnit();
            if (unit == null)
                return;

            IconStateChange();

            MapCtx.OnUnitDeselect?.Invoke(unit);
        }
    }
}
