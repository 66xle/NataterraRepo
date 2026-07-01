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

    private FactionSettings _factionSettings;
    private Dictionary<UnitType, UnitData> _dictUnits;

    public FactionSettings FactionSetting { get { return _factionSettings; } }

    public ServerMapWrapper(List<HexCellState> state, int[] basesPlaced, Dictionary<UnitType, UnitData> dictUnits)
    {
        _state = state;
        _basesPlaced = basesPlaced;
        _dictUnits = dictUnits;
        _stateChanges = new();
    }

    public void SetFactionSettings(FactionSettings factionSettings)
    {
        _factionSettings = factionSettings;
    }

    public GameObject AddUnit(UnitType type, int cellIndex, bool stateChange = true)
    {
        if (stateChange)
            AddStateChange(cellIndex);

        UnitData unitData = GetUnitData(type);
        Unit unit = new Unit(unitData);

        if (_state[cellIndex].DictOfGroups.TryGetValue(type, out Group group))
        {
            group.ListOfUnits.Add(unit);
            return unitData.Prefab;
        }

        _state[cellIndex].DictOfGroups.Add(type, new Group(unit));
        return unitData.Prefab;
    }

    public List<GameObject> AddUnit(List<UnitType> types, int cellIndex)
    {
        AddStateChange(cellIndex);

        List<GameObject> tempObjs = new();

        foreach (UnitType unit in types)
        {
            GameObject unitObj = AddUnit(unit, cellIndex, false);
            tempObjs.Add(unitObj);
        }

        return tempObjs;
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


    private void AddStateChange(int cellIndex)
    {
        if (!_stateChanges.Contains(cellIndex))
        {
            _stateChanges.Add(cellIndex);
        }
    }

    private UnitData GetUnitData(UnitType type)
    {
        if (!_dictUnits.TryGetValue(type, out UnitData data))
        {
            Debug.LogError($"{type} data does not exist in database");
            return null;
        }

        return data;
    }
}
