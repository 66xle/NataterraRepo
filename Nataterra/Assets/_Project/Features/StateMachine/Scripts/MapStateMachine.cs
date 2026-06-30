using PurrNet;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TGS;
using UnityEngine;

public class MapStateMachine : MonoBehaviour
{
    [Header("References")]
    public GameplaySystem GS;

    TerrainGridSystem _tgs;
    ServerMap _serverMap;
    List<HexCellState> _state;

    public void Setup()
    {
        MapData mapData = GameManager.Instance.MapData;

        _state = new();
        foreach (HexCellData data in mapData.hexCells)
        {
            _state.Add(new HexCellState(data));
        }

        SetupGrid(mapData);
        SetupServerMap(mapData);
    }

    public void SpawnUnits()
    {
        AC_UnitRecruitCommand command = new AC_UnitRecruitCommand
        {
            Amount = 1,
            Faction = Base.Beasts,
            Unit = Unit.Woodcutter
        };

        _serverMap.HandleCommand(command);
    }

    void SetupServerMap(MapData mapData)
    {
        if (!InstanceHandler.TryGetInstance(out ServerMap serverMap))
        {
            Debug.LogError("StateMachineManager: Failed to get Server Manager Instance");
            return;
        }

        _serverMap = serverMap;
        _serverMap.Init(new ServerMapWrapper(_state, mapData.bases), GS);
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

}
