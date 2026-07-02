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

    FactionSettings _settings;
    Dictionary<UnitType, UnitData> _dictOfUnits;


    Cell _selectedCell;
    GameObject _movementBorder;

    public Cell SelectedCell { get { return _selectedCell; } set { _selectedCell = value; } }
    public GameObject MovementBorder { get { return _movementBorder; } set { _movementBorder = value; } }

    public Action<List<Unit>> OnSelectUnit;



    public void Setup(Dictionary<UnitType, UnitData> dictUnits)
    {
        _dictOfUnits = dictUnits;
        _state = new();

        MapData mapData = GameManager.Instance.MapData;
        foreach (HexCellData data in mapData.hexCells)
        {
            _state.Add(new HexCellState(data));
        }

        SetupGrid(mapData);
        SetupServerMap(mapData);
    }

    void SetupServerMap(MapData mapData)
    {
        if (!InstanceHandler.TryGetInstance(out ServerMap serverMap))
        {
            Debug.LogError("StateMachineManager: Failed to get Server Manager Instance");
            return;
        }

        ServerMapWrapper wrapper = new ServerMapWrapper(_state, mapData.bases, _dictOfUnits);
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


    public void SetFactionSetting(FactionSettings settings)
    {
        _settings = settings;
        _serverMap.SetFactionSetting(settings);
    }

    [ServerRpc]
    public void SpawnStartingUnits(Base faction, RPCInfo info = default)
    {
        AC_InitialUnitSpawnCommand command = new AC_InitialUnitSpawnCommand
        {
            Faction = faction
        };

        _serverMap.HandleCommand(command);
    }

    public void SetCellState(HexCellState cellState, int cellIndex)
    {
        _state[cellIndex] = cellState;
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
}
