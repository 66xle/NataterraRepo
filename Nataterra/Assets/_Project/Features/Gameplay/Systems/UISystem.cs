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

    public void Setup()
    {
        _resourceUI = InstanceHandler.GetInstance<ResourceUI>();
        _uiManager = InstanceHandler.GetInstance<UIManager>();

        if (isServer) return;

        UpdateResourcesUI();
    }

    [TargetRpc]
    public void ResourceUpdateClientUI(PlayerID playerID)
    {
        UpdateResourcesUI();
    }

    void UpdateResourcesUI()
    {
        int food = _gs.MSM.FactionState.Food;
        int wood = _gs.MSM.FactionState.Wood;
        int metal = _gs.MSM.FactionState.Metal;

        SetResourceUI(food, wood, metal);
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
        ShowPhaseTitle(state);
    }

    [ObserversRpc]
    public void ShowPhaseTitleToAll(GameplayState state)
    {
        ShowPhaseTitle(state);
    }

    public void ShowPhaseTitle(GameplayState state)
    {
        if (state == GameplayState.MovementPhase)
            _uiManager.ShowPhaseUI("Movement Phase");
        else if (state == GameplayState.ResourcePhase)
            _uiManager.ShowPhaseUI("Resource Phase");
        else if (state == GameplayState.CombatPhase)
            _uiManager.ShowPhaseUI("Combat Phase");
        else if (state == GameplayState.DevelopmentPhase)
            _uiManager.ShowPhaseUI("Development Phase");
    }


    [ObserversRpc]
    public void ShowFactionTurn(Base faction)
    {
        _uiManager.ShowFactionTurn($"{faction}'s Turn");
    }
}
