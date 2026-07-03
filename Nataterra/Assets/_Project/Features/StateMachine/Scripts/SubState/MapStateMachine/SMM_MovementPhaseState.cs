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
        int currentMovement = units[0].CurrentMovement;

        // Get the lowest avaliable movement
        for (int i = 1; i < units.Count; i++)
        {
            if (units[i].CurrentMovement < currentMovement)
                currentMovement = units[i].CurrentMovement;
        }

        if (currentMovement == 0)
        {
            // No movement message
            return;
        }

        

        DijkstraResult result = MapCtx.GS.CalculateMovementRange(MapCtx.SelectedCell.index, currentMovement);

        List<int> cellsInRange = result.Cost.Keys.ToList();
        //List<int> cellsInRange = TGS.CellGetNeighboursWithinRangeHex(MapCtx.SelectedCell.index, 1, currentMovement);

        MapCtx.CellsWithinMovement = cellsInRange.ToHashSet();

        Debug.Log($"Number of cells in range: {cellsInRange.Count}");

        if (MapCtx.MovementBorder != null)
            Destroy(MapCtx.MovementBorder);

        cellsInRange.Add(MapCtx.SelectedCell.index);
        MapCtx.MovementBorder = TGS.CellDrawBorder(cellsInRange, Color.green);
    }

    private void MoveUnit()
    {
        if (MapCtx.CellsWithinMovement.Count == 0)
            return;

        Cell cell = TGS.CellGetAtMousePosition();
        if (cell == null) return;

        if (!MapCtx.CellsWithinMovement.Contains(cell.index))
        {
            // Display UI error message 
            return;
        }
        
        // Send Command

    }
}
