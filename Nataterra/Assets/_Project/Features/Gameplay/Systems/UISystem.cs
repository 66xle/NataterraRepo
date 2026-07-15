using UnityEngine;
using PurrNet;

public class UISystem : NetworkBehaviour
{
    GameplaySystem _gs;
    ResourceUI _resourceUI;

    private void Awake()
    {
        _gs = GetComponentInParent<GameplaySystem>();
    }

    public void Setup()
    {
        _resourceUI = InstanceHandler.GetInstance<ResourceUI>();

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

        _resourceUI.SetResource(food, wood, metal);
    }
}
