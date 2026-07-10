using PurrNet;
using System.Collections.Generic;
using TGS;
using UnityEngine;

public class GameplaySystem : NetworkBehaviour
{
    public MapStateMachine MSM;

    public UnitSystem UnitSystem { get; private set; }

    public TerrainGridSystem TGS { get; private set; }

    public void Setup()
    {
        UnitSystem = GetComponentInChildren<UnitSystem>();

        TGS = TerrainGridSystem.instance; 
    }

    [ObserversRpc]
    public void SetLocalChanges(List<StateChange> changes)
    {
        foreach (StateChange state in changes)
        {
            MSM.SetCellState(state.State, state.CellIndex);
        }
    }
}
