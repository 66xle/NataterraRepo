using System.Collections;
using System.Collections.Generic;
using TGS;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    public MapStateMachine MapCtx;
    public CombatStateMachine CombatCtx;

    GameplayBaseState _currentState;
    GameplayStateFactory _states;

    public GameplayBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

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
        if (_currentState == null) return;

        _currentState.UpdateStates();
    }

}
