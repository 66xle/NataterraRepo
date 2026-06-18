using TGS;
using UnityEngine;
using System.Collections.Generic;

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
        tgs = Terrain.activeTerrain.gameObject.AddComponent<TerrainGridSystem>();
        tgs.rowCount = 8;
        tgs.columnCount = 8;
        tgs.gridTopology = GridTopology.Hexagonal;
        tgs.SetGridType(GridTopology.Irregular);
        tgs.ToggleTerritories(false);

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
