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

    public void Setup()
    {
        UnitSystem = GetComponentInChildren<UnitSystem>();
        UISystem = GetComponentInChildren<UISystem>();

        TGS = TerrainGridSystem.instance;


        UISystem.Setup();
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
