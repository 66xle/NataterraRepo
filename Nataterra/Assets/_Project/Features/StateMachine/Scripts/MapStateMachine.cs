using PurrNet;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
        _tgs.SetGridType(GridTopology.Irregular);
        _tgs.SetGridSize(mapData.row, mapData.column);
        _tgs.ToggleTerritories(false);
        _tgs.highlightMode = HighlightMode.None;

        _tgs.Redraw();

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
}
