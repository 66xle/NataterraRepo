using PurrNet;
using System.Collections.Generic;
using TGS;
using UnityEngine;

public class MapStateMachine : MonoBehaviour
{
    TerrainGridSystem tgs;

    [ServerOnly]
    public void Setup()
    {
        Debug.Log("Map Setup");

        GenerateGrid();
    }

    [ServerOnly]
    void GenerateGrid()
    {
        tgs = Terrain.activeTerrain.gameObject.AddTerrainGridSystem();

        tgs.gridTopology = GridTopology.Hexagonal;
        tgs.SetGridType(GridTopology.Irregular);
        tgs.SetGridSize(8, 8);
        tgs.ToggleTerritories(false);
        tgs.highlightMode = HighlightMode.None;

        tgs.Redraw();

        MapData mapData = GameManager.Instance.MapData;

        tgs.RegenerateFlatToppedHexagonalGrid(mapData.tgsCells);
        tgs.CellsUpdateBounds();
        tgs.CellsUpdateNeighbours();
        tgs.RedrawCells(tgs.cells);
    }
    

}
