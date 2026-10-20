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
    UIManager UIManager;

    [SerializeField] Material XrayMaterial;
    [SerializeField] Image SelectedImage;
    [SerializeField] Image TextureImage;
    [SerializeField] TMP_Text RemainingUnitText;
    [SerializeField] TMP_Text SelectedUnitText;

    UnitType _unitType;
    Dictionary<int, List<Unit>> _dictMovementUnits;
    Dictionary<int, List<Unit>> _dictSelectedUnits = new();

    List<int> _listMovements = new();

    public List<int> ListOfMovement => _listMovements;
    public Dictionary<int, List<Unit>> DictMovementUnits => _dictMovementUnits;
    public Dictionary<int, List<Unit>> DictSelectedUnits => _dictSelectedUnits;
    public UnitType GetUnitType() => _unitType;
    public Texture GetTexture() => TextureImage.mainTexture;

    private void Start()
    {
        MapCtx = InstanceHandler.GetInstance<MapStateMachine>();
        UIManager = InstanceHandler.GetInstance<UIManager>();
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
        _dictSelectedUnits.Clear();

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

    Unit SelectUnit(int movement = -1)
    {
        List<int> temp = new();
        if (movement == -1)
        {
            temp = _listMovements;
        }
        else
        {
            temp.Add(movement);
        }

        foreach (int movementIndex in temp)
        {
            if (_dictMovementUnits[movementIndex].Count == 0)
                continue;

            Unit selectedUnit = _dictMovementUnits[movementIndex][0];
            _dictMovementUnits[movementIndex].Remove(selectedUnit);

            if (_dictSelectedUnits.ContainsKey(movementIndex))
            {
                _dictSelectedUnits[movementIndex].Add(selectedUnit);
            }
            else
            {
                _dictSelectedUnits[movementIndex] = new List<Unit> { selectedUnit };
            }

            return selectedUnit;
        }

        return null;
    }

    Unit DeselectUnit(int movement = -1)
    {
        List<int> temp = new();
        if (movement == -1)
        {
            temp = _listMovements;
        }
        else
        {
            temp.Add(movement);
        }

        for (int i = temp.Count - 1; i >= 0; i--)
        {
            int movementIndex = temp[i];

            if (!_dictSelectedUnits.ContainsKey(movementIndex))
                continue;

            if (_dictSelectedUnits[movementIndex].Count == 0)
                continue;

            Unit unit = _dictSelectedUnits[movementIndex][0];
            _dictSelectedUnits[movementIndex].Remove(unit);
            _dictMovementUnits[movementIndex].Add(unit);

            return unit;
        }

        return null;
    }

    void IconStateChange(UnitSelectionPopup popup = null, int movement = -1)
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

        SelectedUnitText.text = count.ToString();
        RemainingUnitText.text = CheckDictSize(_dictMovementUnits).ToString();

        if (popup != null)
        {
            int selected = _dictSelectedUnits[movement].Count;
            int remaining = _dictMovementUnits[movement].Count;

            popup.IconStateChange(selected, remaining);
        }
    }

    public void SelectIcon(UnitSelectionPopup popup = null, int movement = -1)
    {
        Unit unit = SelectUnit(movement);
        if (unit == null)
            return;

        IconStateChange(popup, movement);

        MapCtx.OnUnitSelect?.Invoke(unit);
    }

    public void DeselectIcon(UnitSelectionPopup popup = null, int movement = -1)
    {
        Unit unit = DeselectUnit(movement);
        if (unit == null)
            return;

        IconStateChange(popup, movement);

        MapCtx.OnUnitDeselect?.Invoke(unit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectIcon();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            DeselectIcon();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            UIManager.ShowSelectionPopup(true, this);
        }
    }
}
