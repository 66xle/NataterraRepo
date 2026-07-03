using PurrNet.Packing;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public GameObject AddUnit(UnitType type, int cellIndex, Unit unit = null, bool stateChange = true)
    {
        if (stateChange)
            AddStateChange(cellIndex);

        UnitData unitData = GetUnitData(type);

        if (unit == null)
            unit = new Unit(unitData, cellIndex);


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
            GameObject unitObj = AddUnit(unit, cellIndex, null, false);
            tempObjs.Add(unitObj);
        }

        return tempObjs;
    }


    public void RemoveUnit(UnitType type, Unit unit, int cellIndex, bool stateChange = true)
    {
        if (stateChange)
            AddStateChange(cellIndex);

        if (!_state[cellIndex].DictOfGroups.TryGetValue(type, out Group group))
        {
            Debug.LogError("Server Map Wrapper: RemoveUnit(): Unit Type not found");
            return;
        }

        if (group.ListOfUnits.Count > 1)
        {
            // Remove unit
            group.ListOfUnits.Remove(unit);
            return;
        }

        // Remove group
        _state[cellIndex].DictOfGroups.Remove(type);
    }


    public void MoveUnit(List<Unit> units, int origin, int destination, int cost)
    {
        AddStateChange(origin);
        AddStateChange(destination);

        foreach (Unit unit in units)
        {
            // remove unit from cell
            RemoveUnit(unit.UnitType, unit, origin, false);

            // Set unit movement
            unit.CurrentMovement -= cost;

            if (unit.CurrentMovement < 0)
            {
                Debug.LogError("Server Map Wrapper: MoveUnit(): Path cost is higher than Unit's movement");
            }

            // add them to map
            AddUnit(unit.UnitType, destination, unit, false);
        }
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

    public List<Unit> GetUnits(int cellIndex, List<string> guids, List<UnitType> types)
    {
        List<Unit> units = new();

        for (int i = 0; i < guids.Count; i++)
        {
            if (!_state[cellIndex].DictOfGroups.TryGetValue(types[i], out Group group))
            {
                Debug.LogError("Server Map Wrapper: GetUnits(): Unit Type not found");
                return null;
            }

            if (!Extensions.Contains(group.ListOfUnits, guids[i], g => g.GUID, out Unit unit))
            {
                Debug.LogError("Server Map Wrapper: GetUnits(): Unit's GUID not found");
                return null;
            }

            units.Add(unit);
        }

        return units;
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
