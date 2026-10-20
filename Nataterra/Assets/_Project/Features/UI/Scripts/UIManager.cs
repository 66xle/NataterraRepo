using Alchemy.Serialization;
using PurrNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HunterGoodin.DynamicObjectPooler;

[AlchemySerialize]
public partial class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text PhaseTitle;
    [SerializeField] Button EndPhaseButton;
    
    [Header("Development")]
    [SerializeField] GameObject UnitUIPrefab;
    [SerializeField] Transform UnitUIParent;

    [Header("Selection")]
    [SerializeField] GameObject UnitSelectionIcon;
    [SerializeField] Transform UnitSelectionParent;
    [SerializeField] GameObject SelectionPopup;
    [SerializeField] GameObject PopupSelectionIcon;
    [SerializeField] Transform PopupContent;

    [AlchemySerializeField, NonSerialized]
    public Dictionary<UnitType, Sprite> DictUnitTextures = new();

    List<UnitUI> _listOfUnitUI = new();

    MapStateMachine MapCtx;
    UnitSelectionRadial _radical;
    List<UnitSelectionIcon> _icons = new();
    List<GameObject> _popups = new();

    private void Awake()
    {
        InstanceHandler.RegisterInstance(this);

        _radical = UnitSelectionParent.GetComponent<UnitSelectionRadial>();
    }

    private void OnDestroy()
    {
        InstanceHandler.UnregisterInstance<UIManager>();
    }

    public void Setup()
    {
        MapCtx = InstanceHandler.GetInstance<MapStateMachine>();
    }

    public void SpawnUnitUI(UnitData data)
    {
        // Spawn UI
        UnitUI ui = Instantiate(UnitUIPrefab, UnitUIParent).GetComponent<UnitUI>();
        ui.SetData(data);
        _listOfUnitUI.Add(ui);
    }

    public void UpdateUnitAvaliable(UnitType type, int amount)
    {
        int index = ((int)type / 10) % 10;

        _listOfUnitUI[index].SetUnitAvailiable(amount);
    }


    public async Task ShowPhaseUI(string text)
    {
        PhaseTitle.text = text;
        PhaseTitle.transform.parent.gameObject.SetActive(true);

        await Task.Delay(1000);

        PhaseTitle.transform.parent.gameObject.SetActive(false);
    }

    public async Task ShowFactionTurn(string text)
    {
        PhaseTitle.text = text;
        PhaseTitle.transform.parent.gameObject.SetActive(true);

        await Task.Delay(1000);

        PhaseTitle.transform.parent.gameObject.SetActive(false);
    }

    

    public void TriggerEndPhaseButton()
    {
        // Disable button
        EnableEndPhaseButton(false);

        MapCtx.OnRequestEndPhase?.Invoke();
    }

    public void EnableEndPhaseButton(bool value)
    {
        EndPhaseButton.interactable = value;
    }


    public void ShowSelectionIcons(Dictionary<UnitType, Group> dictTypes, Vector3 spawnPosition)
    {
        List<UnitSelectionIcon> icons = new();

        foreach (UnitType type in dictTypes.Keys)
        {
            Dictionary<int, List<Unit>> dictMovementUnits = new();

            Group group = dictTypes[type];

            List<Unit> units = new();
            foreach (Unit unit in group.ListOfUnits)
            {
                if (Extensions.Contains(units, unit.CurrentMovement, u => u.CurrentMovement, out Unit found))
                    continue;

                if (dictMovementUnits.ContainsKey(unit.CurrentMovement))
                {
                    dictMovementUnits[unit.CurrentMovement].Add(unit);
                }
                else
                {
                    dictMovementUnits[unit.CurrentMovement] = new List<Unit> { unit };
                }

                units.Add(unit);
            }

            Vector3 dir = spawnPosition - Camera.main.transform.position;

            GameObject icon = ObjectPoolManager.Instance.SpawnObject(UnitSelectionIcon, UnitSelectionParent, Quaternion.identity, ObjectPoolManager.PoolType.SelectionIcon);

            UnitSelectionIcon selectionIcon = icon.GetComponent<UnitSelectionIcon>();
            selectionIcon.SetData(DictUnitTextures[type].texture, type, dictMovementUnits);

            icons.Add(selectionIcon);
        }

        _icons = icons;
        _radical.SetIcons(icons, spawnPosition);
    }

    public void ClearSelectionIcons()
    {
        if (_icons.Count == 0) 
            return;

        foreach (UnitSelectionIcon icon in _icons)
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(icon.gameObject, ObjectPoolManager.PoolType.SelectionIcon);
        }

        _icons.Clear();
        ShowSelectionPopup(false);
    }

    public void ShowSelectionPopup(bool active, UnitSelectionIcon icon = null)
    {
        SelectionPopup.SetActive(active);

        if (!active)
            return;

        foreach (GameObject obj in _popups)
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(obj, ObjectPoolManager.PoolType.SelectionIcon);
        }
        _popups.Clear();

        foreach (int movement in icon.ListOfMovement)
        {
            GameObject popup = ObjectPoolManager.Instance.SpawnObject(PopupSelectionIcon, PopupContent, Quaternion.identity, ObjectPoolManager.PoolType.SelectionIcon);
            _popups.Add(popup);

            UnitSelectionPopup unitSelectionPopup = popup.GetComponent<UnitSelectionPopup>();
            unitSelectionPopup.SetData(icon, movement, DictUnitTextures[icon.GetUnitType()]);

        }
    }
}
