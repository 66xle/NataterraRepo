using PurrNet;
using System.Collections.Generic;
using System.Linq;
using TGS;
using UnityEngine;

public class SMM_MovementPhaseState : GameplayBaseState
{
    public SMM_MovementPhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) 
    {
        
    }
    public override void EnterState()
    {
        Debug.Log("Entered Movement State");

        MapCtx.OnSelectUnit += ShowUnitMovementRange;
        InputManager.Instance.OnRightClickEvent += MoveUnit;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        MapCtx.OnSelectUnit -= ShowUnitMovementRange;
        InputManager.Instance.OnRightClickEvent -= MoveUnit;
    }

    public override void CheckSwitchState()
    {

    }

    public override void InitializeSubState() { }

    private void ShowUnitMovementRange(List<Unit> units)
    {
        int origin = MapCtx.GetOrigin(units, MapCtx.SelectedCell.index);

        int lowestMovement = MapCtx.GetLowestMovement(units);

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
            Destroy(MapCtx.MovementBorder);

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

        MapCtx.SendMoveCommand(cell.index);
    }

    
}
