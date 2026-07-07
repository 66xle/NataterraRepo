using PurrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using TGS;
using UnityEngine;

public class MapStateMachine : NetworkBehaviour
{
    [Header("References")]
    public GameplaySystem GS;

    TerrainGridSystem _tgs;
    ServerMap _serverMap;
    List<HexCellState> _state;
    Dictionary<string, GameObject> _unitObjects;

    Dictionary<UnitType, UnitData> _dictOfUnits;

    Cell _selectedCell;
    List<Unit> _selectedUnits;

    GameObject _movementBorder;

    DijkstraResult _movementResult;

    public Cell SelectedCell { get { return _selectedCell; } set { _selectedCell = value; } }
    public List<Unit> SelectedUnits { get { return _selectedUnits; } set { _selectedUnits = value; } }
    public GameObject MovementBorder { get { return _movementBorder; } set { _movementBorder = value; } }
    public DijkstraResult MovementResult { get { return _movementResult; } set { _movementResult = value; } }


    public Action<List<Unit>> OnSelectUnit;

    
    public void Setup(Dictionary<UnitType, UnitData> dictUnits)
    {
        _dictOfUnits = dictUnits;
        _state = new();
        _unitObjects = new();

        MapData mapData = GameManager.Instance.MapData;
        foreach (HexCellData data in mapData.hexCells)
        {
            _state.Add(new HexCellState(data));
        }

        SetupGrid(mapData);
        SetupServerMap(mapData);

        SetupDataToClients(_serverMap);
    }

    void SetupServerMap(MapData mapData)
    {
        if (!InstanceHandler.TryGetInstance(out ServerMap serverMap))
        {
            Debug.LogError("StateMachineManager: Failed to get Server Manager Instance");
            return;
        }

        List<FactionData> factionData = GameManager.Instance.ListOfFactions;

        ServerMapWrapper wrapper = new ServerMapWrapper(_state, _tgs.cells, mapData.bases, factionData, _dictOfUnits);
        _serverMap = serverMap;
        _serverMap.Init(wrapper, GS);
    }

    void SetupGrid(MapData mapData)
    {
        _tgs = Terrain.activeTerrain.gameObject.AddTerrainGridSystem();

        _tgs.gridTopology = GridTopology.Hexagonal;
        _tgs.SetGridSize(mapData.row, mapData.column);
        _tgs.SetNumTerritories(0, false);
        _tgs.highlightMode = HighlightMode.None;

        _tgs.Redraw();

        _tgs.SetGridType(GridTopology.Irregular);
        _tgs.RegenerateFlatToppedHexagonalGrid(mapData.tgsCells);
        _tgs.CellsUpdateBounds();
        _tgs.CellsUpdateNeighbours();
        _tgs.RedrawCells(_tgs.cells);
    }


    [ObserversRpc(bufferLast: true)]
    void SetupDataToClients(ServerMap serverMap)
    {
        _state = serverMap.Map.GetNewState();
        _dictOfUnits = serverMap.Map.DictOfUnits;

        if (_tgs == null)
            _tgs = TerrainGridSystem.instance;
    }

    [ServerRpc]
    public void SpawnStartingUnits(Base faction, RPCInfo info = default)
    {
        AC_UnitInitialSpawnCommand command = new AC_UnitInitialSpawnCommand
        {
            Faction = faction
        };

        _serverMap.HandleCommand(command);
    }

    [ServerRpc]
    public void SendMoveCommand(int destination)
    {
        List<string> guids = new();
        List<UnitType> type = new();
        foreach (Unit unit in SelectedUnits)
        {
            guids.Add(unit.GUID);
            type.Add(unit.UnitType);
        }

        // Send Command
        AC_UnitMoveCommand command = new AC_UnitMoveCommand()
        {
            ListOfUnitType = type,
            ListOfUnitGUID = guids,
            SelectedIndex = SelectedCell.index,
            Destination = destination
        };

        _serverMap.HandleCommand(command);
    }



    public void SetCellState(HexCellState cellState, int cellIndex)
    {
        _state[cellIndex] = cellState;
    }


    public void AddUnitObject(string guid, GameObject obj)
    {
        _unitObjects.Add(guid, obj);
    }

    public GameObject GetUnitObject(string guid)
    {
        return _unitObjects[guid];
    }

    public bool UnitExistOnCell(int cellIndex)
    {
        return _state[cellIndex].DictOfGroups.Count == 0 ? false : true;
    }

    public List<Unit> GetUnitList(int cellIndex)
    {
        List<UnitType> unitTypes = _state[cellIndex].DictOfGroups.Keys.ToList();

        return _state[cellIndex].DictOfGroups[unitTypes[0]].ListOfUnits;
    }


    public int GetOrigin(List<Unit> units, int selectedIndex, out bool IsOrigin)
    {
        int origin = units[0].CellOrigin;
        IsOrigin = true;

        for (int i = 1; i < units.Count; i++)
        {
            if (units[i].CellOrigin == origin)
                continue;

            IsOrigin = false;
            return selectedIndex;
        }

        return origin;
    }

    public int GetLowestMovement(List<Unit> units, bool isOrigin)
    {
        if (isOrigin)
            return units[0].Movement;

        int currentMovement = units[0].CurrentMovement;

        // Get the lowest avaliable movement
        for (int i = 1; i < units.Count; i++)
        {
            if (units[i].CurrentMovement < currentMovement)
                currentMovement = units[i].CurrentMovement;
        }

        return currentMovement;
    }

    public DijkstraResult CalculateMovementRange(int startCell, int maxMovement, List<Cell> cells)
    {
        DijkstraResult result = new DijkstraResult();

        List<int> open = new();

        open.Add(startCell);

        result.Cost[startCell] = 0;
        result.Parent[startCell] = -1;

        while (open.Count > 0)
        {
            // Find the node with the lowest cost
            int current = open[0];

            for (int i = 1; i < open.Count; i++)
            {
                if (result.Cost[open[i]] < result.Cost[current])
                    current = open[i];
            }

            open.Remove(current);

            int currentCost = result.Cost[current];

            Cell cell = cells[current];

            foreach (Cell neighbour in cell.neighbours)
            {
                if (!CanEnter(neighbour))
                    continue;

                int newCost = currentCost + GetMovementCost(neighbour);

                if (newCost > maxMovement)
                    continue;

                if (!result.Cost.TryGetValue(neighbour.index, out int oldCost) ||
                    newCost < oldCost)
                {
                    result.Cost[neighbour.index] = newCost;
                    result.Parent[neighbour.index] = current;

                    if (!open.Contains(neighbour.index))
                        open.Add(neighbour.index);
                }
            }
        }

        return result;
    }

    private bool CanEnter(Cell cell)
    {
        if (!cell.canCross)
            return false;

        return true;
    }

    private int GetMovementCost(Cell cell)
    {
        // Check if ground or flying type

        // If ground check if cell is a mountain

        return 1;
    }

}
