using PurrNet;
using System.Collections.Generic;
using System.Linq;
using TGS;
using UnityEngine;

public class SMM_MovementPhaseState : GameplayBaseState
{
    public SMM_MovementPhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
    public override void EnterState()
    {
        Debug.Log("Entered Movement State");

        MapCtx.OnEndPhase += SwitchToResourcePhase;

        MapCtx.OnSelectUnit += ShowUnitMovementRange;
        InputManager.Instance.OnRightClickEvent += MoveUnit;
    }
    public override void UpdateState() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapCtx.SendEndPhaseCommand(GameplayState.MovementPhase);
        }
    }
    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        MapCtx.OnEndPhase -= SwitchToResourcePhase;

        MapCtx.OnSelectUnit -= ShowUnitMovementRange;
        InputManager.Instance.OnRightClickEvent -= MoveUnit;
    }

    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }

    void SwitchToResourcePhase()
    {
        SwitchState(Factory.ResourcePhase());
    }

    private void ShowUnitMovementRange(List<Unit> units)
    {
        int origin = MapCtx.GetOrigin(units, MapCtx.SelectedCell.index, out bool IsOrigin);

        int lowestMovement = MapCtx.GetLowestMovement(units, IsOrigin);

        if (lowestMovement == 0)
        {
            // No movement message
            return;
        }

        MapCtx.SelectedUnits = units;

        DijkstraResult result = MapCtx.CalculateMovementRange(origin, lowestMovement, TGS.cells);
        MapCtx.MovementResult = result;

        List<int> cellsInRange = result.GetIndexList();

        Debug.Log($"Number of cells in range: {cellsInRange.Count}");

        if (MapCtx.MovementBorder != null)
            MapCtx.RemoveMovementBorder();

        cellsInRange.Add(MapCtx.SelectedCell.index);
        MapCtx.MovementBorder = TGS.CellDrawBorder(cellsInRange, Color.green);
    }

    private void MoveUnit()
    {
        if (MapCtx.MovementResult == null)
            return;

        Cell cell = TGS.CellGetAtMousePosition();

        if (cell == null) 
            return;

        if (!MapCtx.MovementResult.Contains(cell.index))
        {
            // Display UI invalid destination
            return;
        }

        if (MapCtx.SelectedUnits.Count == 0)
        {
            Debug.LogError("MovementPhaseState: MoveUnit(): No units are selected");
            return;
        }

        MapCtx.SendMoveCommand(MapCtx.SelectedCell.index, cell.index, MapCtx.SelectedUnits);

        MapCtx.MovementResult = null;
        MapCtx.RemoveMovementBorder();

        TGS.CellDestroyBorder(MapCtx.SelectedCell.index);
        MapCtx.SelectedCell = null;
    }
}
