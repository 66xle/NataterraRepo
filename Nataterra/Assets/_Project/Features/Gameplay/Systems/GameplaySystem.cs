using PurrNet;
using System.Collections.Generic;
using TGS;
using UnityEngine;

public class GameplaySystem : NetworkBehaviour
{
    public MapStateMachine MSM;

    public UnitSystem UnitSystem { get; private set; }
    public UISystem UISystem { get; private set; }

    public TerrainGridSystem TGS { get; private set; }

    private void Awake()
    {
        UnitSystem = GetComponentInChildren<UnitSystem>();
        UISystem = GetComponentInChildren<UISystem>();
    }

    public void Setup()
    {
        TGS = TerrainGridSystem.instance;
    }

    [ObserversRpc]
    public void SetStateChanges(List<StateChange> changes)
    {
        foreach (StateChange state in changes)
        {
            MSM.SetCellState(state.State, state.CellIndex);
        }
    }

    [TargetRpc]
    public void SetClientFactionState(PlayerID playerID, FactionState state)
    {
        MSM.SetFactionState(state);
    }
}
