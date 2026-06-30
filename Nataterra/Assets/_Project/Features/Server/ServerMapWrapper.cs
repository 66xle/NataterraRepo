using DrawXXL;
using System.Collections.Generic;
using UnityEngine;

public struct StateChange
{
    public HexCellState State;
    public int CellIndex;

    public StateChange(HexCellState state, int cellIndex)
    {
        State = state;
        CellIndex = cellIndex;
    }
}


public class ServerMapWrapper
{
    private List<HexCellState> _state;
    private int[] _basesPlaced;

    private List<int> _stateChanges;

    public ServerMapWrapper(List<HexCellState> state, int[] basesPlaced)
    {
        _state = state;
        _basesPlaced = basesPlaced;
        _stateChanges = new();
    }

    public void AddUnit(Unit unit, int amount, int cellIndex)
    {
        if (!_stateChanges.Contains(cellIndex))
        {
            _stateChanges.Add(cellIndex);
        }

        // If unit is in group
        if (Extensions.Contains(_state[cellIndex].listOfGroups, unit, g => g.Unit, out Group group))
        {
            group.Amount += amount;
            return;
        }

        _state[cellIndex].listOfGroups.Add(new Group(unit, amount));
    }


    public List<StateChange> GetStateChanges()
    {
        List<StateChange> state = new();

        foreach (int index in _stateChanges)
        {
            state.Add(new StateChange(_state[index], index));
        }

        _stateChanges.Clear();
        return state;
    }

    public int GetBaseCellIndex(Base faction)
    {
        return _basesPlaced[(int)faction];
    }
}
