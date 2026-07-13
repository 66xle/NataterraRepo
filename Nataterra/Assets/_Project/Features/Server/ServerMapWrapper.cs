using PurrNet;
using PurrNet.Packing;
using System.Collections.Generic;
using TGS;
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
    private List<int> _stateChanges;
    private List<Cell> _cells;
    private int[] _basesPlaced;
    private GameplayState _phaseState;
    private PlayerID _currentPlayerTurn;

    private Dictionary<Base, FactionSettings> _factionSettings;
    private Dictionary<UnitType, UnitData> _dictOfUnits;
    private Dictionary<string, DijkstraResult> _results;
    private Dictionary<PlayerID, Base> _dictFaction;
    private Dictionary<Base, FactionState> _factionState;

    public Dictionary<UnitType, UnitData> DictOfUnits { get { return _dictOfUnits; } }
    public Dictionary<Base, FactionSettings> FactionSettings { get { return _factionSettings; } }
    public PlayerID CurrentPlayerTurn { get { return _currentPlayerTurn; } }

    public ServerMapWrapper(List<HexCellState> state, List<Cell> cells, int[] basesPlaced, List<FactionData> factionData, Dictionary<UnitType, UnitData> dictOfUnits)
    {
        _state = state;
        _cells = cells;
        _basesPlaced = basesPlaced;
        _dictOfUnits = dictOfUnits;
        _stateChanges = new();
        _factionSettings = new();
        _factionState = new();
        _results = new();
        _dictFaction = new();

        _phaseState = GameplayState.WaitingForTurn;

        foreach (FactionData data in factionData)
        {
            FactionSettings setting = data.Settings;
            _factionSettings.Add(setting.Faction, setting);
            _factionState.Add(setting.Faction, new FactionState(setting.ListOfUnitAvaliable));
        }
    }

    public void AddFaction(PlayerID playerID, Base facton)
    {
        _dictFaction.Add(playerID, facton);

        if (_dictFaction.Count == 1)
        {
            _currentPlayerTurn = playerID;
        }
    }

    public Base GetFaction(PlayerID playerID)
    {
        return _dictFaction[playerID];
    }
    public FactionState GetFactionState(PlayerID playerID)
    {
        return _factionState[GetFaction(playerID)];
    }

    public void AddResource(Base faction, Resource resource)
    {
        if (resource == Resource.Food)
        {
            _factionState[faction].Food++;
        }
        else if (resource == Resource.Wood)
        {
            _factionState[faction].Wood++;
        }
        else if (resource == Resource.Metal)
        {
            _factionState[faction].Metal++;
        }
    }

    public string AddUnit(UnitType type, int cellIndex, Unit unit = null, bool stateChange = true)
    {
        if (stateChange)
            AddStateChange(cellIndex);

        UnitData unitData = GetUnitData(type);

        if (unit == null)
            unit = new Unit(unitData, cellIndex);

        string GUID = unit.GUID;

        if (_state[cellIndex].DictOfGroups.TryGetValue(type, out Group group))
        {
            group.ListOfUnits.Add(unit);
            return GUID;
        }

        _state[cellIndex].DictOfGroups.Add(type, new Group(unit));
        return GUID;
    }

    public List<string> AddUnit(List<UnitType> types, int cellIndex)
    {
        AddStateChange(cellIndex);

        List<string> tempGUIDS = new();

        foreach (UnitType unit in types)
        {
            string guid = AddUnit(unit, cellIndex, null, false);
            tempGUIDS.Add(guid);
        }

        return tempGUIDS;
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


    public void MoveUnit(List<Unit> units, int origin, int destination, List<DijkstraResult> listOfResults)
    {
        AddStateChange(origin);
        AddStateChange(destination);

        for (int i = 0; i < units.Count; i++)
        {
            Unit unit = units[i];

            // remove unit from cell
            RemoveUnit(unit.UnitType, unit, origin, false);

            // Set unit movement
            int cost = listOfResults[i].GetDestinationCost(destination);
            unit.CurrentMovement = unit.Movement - cost;

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
            state.Add(new StateChange(new HexCellState(_state[index]), index));
        }

        _stateChanges.Clear();
        return state;
    }

    public List<HexCellState> GetState()
    {
        return _state;
    }

    public List<Cell> GetCells()
    {
        return _cells;
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


    public DijkstraResult GetPathfindingResult(string guid)
    {
        if (_results.TryGetValue(guid, out DijkstraResult result))
        {
            return result;
        }

        return null;
    }

    public void StoreResult(string guid, DijkstraResult result)
    {
        _results.Add(guid, result);
    }


    


    public void SetPhaseState(GameplayState state)
    {
        _phaseState = state;
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
        if (!_dictOfUnits.TryGetValue(type, out UnitData data))
        {
            Debug.LogError($"{type} data does not exist in database");
            return null;
        }

        return data;
    }
}
