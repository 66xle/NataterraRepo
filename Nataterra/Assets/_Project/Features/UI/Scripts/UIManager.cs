using Alchemy.Serialization;
using PurrNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [AlchemySerializeField, NonSerialized]
    public Dictionary<UnitType, Texture2D> DictUnitTextures = new();

    List<UnitUI> _listOfUnitUI = new();

    MapStateMachine MapCtx;

    private void Awake()
    {
        InstanceHandler.RegisterInstance(this);
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
        foreach (UnitType type in dictTypes.Keys)
        {
            Vector3 dir = spawnPosition - Camera.main.transform.position;
            GameObject icon = Instantiate(UnitSelectionIcon, spawnPosition, Quaternion.LookRotation(dir), UnitSelectionParent);

            XRayImageTexture xray = icon.GetComponent<XRayImageTexture>();
            xray.SetMaterial(DictUnitTextures[type]);
        }
    }
}
