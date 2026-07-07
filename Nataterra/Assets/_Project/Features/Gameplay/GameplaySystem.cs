using System.Collections.Generic;
using PurrNet;
using TGS;

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

    [ObserversRpc(bufferLast:true)]
    public void SetLocalChanges(List<StateChange> changes)
    {
        foreach (StateChange state in changes)
        {
            MSM.SetCellState(state.State, state.CellIndex);
        }
    }
}
