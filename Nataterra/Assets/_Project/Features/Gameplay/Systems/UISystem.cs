using NUnit.Framework;
using PurrNet;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UISystem : NetworkBehaviour
{
    GameplaySystem _gs;
    ResourceUI _resourceUI;
    UIManager _uiManager;

    private void Awake()
    {
        _gs = GetComponentInParent<GameplaySystem>();
    }

    public void Setup(Base faction)
    {
        _resourceUI = InstanceHandler.GetInstance<ResourceUI>();
        _uiManager = InstanceHandler.GetInstance<UIManager>();

        _uiManager.Setup();
        _gs.AddEventQueue(() => UpdateResourcesUI());

        foreach (FactionData data in GameManager.Instance.ListOfFactions)
        {
            if (faction != data.Settings.Faction)
                continue;

            // Load Units in Development UI
            foreach (UnitData unitData in data.ListOfUnits)
            {
                _uiManager.SpawnUnitUI(unitData);
            }

            break;
        }
    }

    [TargetRpc]
    public void ResourceUpdateClientUI(PlayerID playerID)
    {
        _gs.AddEventQueue(() => UpdateResourcesUI());
    }

    void UpdateResourcesUI()
    {
        int food = _gs.MSM.FactionState.Food;
        int wood = _gs.MSM.FactionState.Wood;
        int metal = _gs.MSM.FactionState.Metal;

        SetResourceUI(food, wood, metal);

        _gs.RemoveEventQueue();
    }

    public void SetResourceUI(int food, int wood, int metal)
    {
        _resourceUI.SetResource(food, wood, metal);
    }



    [TargetRpc]
    public void UnitAvaliableUpdateClientUI(PlayerID playerID, List<UnitType> units)
    {
        foreach (UnitType type in units)
        {
            int amount = _gs.MSM.FactionState.CurrentUnitAvaliable[type];
            SetUnitAvaliable(type, amount);
        }
    }

    public void SetUnitAvaliable(UnitType type, int amount)
    {
        _uiManager.UpdateUnitAvaliable(type, amount);
    }


    [TargetRpc]
    public void ShowPhaseTitleClient(PlayerID playerID, GameplayState state)
    {
        _gs.AddEventQueue(() => ShowPhaseTitle(state));
    }
    [ObserversRpc]
    public void ShowPhaseTitleToAll(GameplayState state)
    {
        _gs.AddEventQueue(() => ShowPhaseTitle(state));
    }
    async void ShowPhaseTitle(GameplayState state)
    {
        if (state == GameplayState.MovementPhase)
            await _uiManager.ShowPhaseUI("Movement Phase");
        else if (state == GameplayState.ResourcePhase)
            await _uiManager.ShowPhaseUI("Resource Phase");
        else if (state == GameplayState.CombatPhase)
            await _uiManager.ShowPhaseUI("Combat Phase");
        else if (state == GameplayState.DevelopmentPhase)
            await _uiManager.ShowPhaseUI("Development Phase");

        _gs.RemoveEventQueue();
    }


    [ObserversRpc]
    public void ShowFactionTurnToAll(Base faction)
    {
        _gs.AddEventQueue(() => ShowFactionTurn(faction));
    }
    async void ShowFactionTurn(Base faction)
    {
        await _uiManager.ShowFactionTurn($"{faction}'s Turn");

        _gs.RemoveEventQueue();
    }


    public void EnableEndPhaseButton(bool value)
    { 
        _uiManager.EnableEndPhaseButton(value);
    }
}
