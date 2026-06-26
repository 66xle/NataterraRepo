using PurrNet;
using System.Collections.Generic;
using TGS;
using UnityEngine;

public class MapStateMachine : MonoBehaviour
{
    public GameplaySystem GS;

    TerrainGridSystem _tgs;
    ServerMap _serverMap;

    public void Setup()
    {
        if (!InstanceHandler.TryGetInstance(out ServerMap serverMap))
        {
            Debug.LogError("StateMachineManager: Failed to get Server Manager Instance");
            return;
        }

        _serverMap = serverMap;

        MapData mapData = GameManager.Instance.MapData;

        SetupGrid(mapData);
        _serverMap.Init(mapData, GS);

        UnitSpawnCommand command = new UnitSpawnCommand
        {
            Amount = 1,
            Faction = Base.Beasts,
            Unit = Unit.Woodcutter
        };

        _serverMap.HandleCommand(command);
    }

    void SetupGrid(MapData mapData)
    {
        _tgs = Terrain.activeTerrain.gameObject.AddTerrainGridSystem();

        _tgs.gridTopology = GridTopology.Hexagonal;
        _tgs.SetGridType(GridTopology.Irregular);
        _tgs.SetGridSize(8, 8);
        _tgs.ToggleTerritories(false);
        _tgs.highlightMode = HighlightMode.None;

        _tgs.Redraw();

        _tgs.RegenerateFlatToppedHexagonalGrid(mapData.tgsCells);
        _tgs.CellsUpdateBounds();
        _tgs.CellsUpdateNeighbours();
        _tgs.RedrawCells(_tgs.cells);
    }

}
