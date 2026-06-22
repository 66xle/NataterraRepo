using TGS;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    TerrainGridSystem tgs;

    void Start()
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

    void Update()
    {
        
    }
}
