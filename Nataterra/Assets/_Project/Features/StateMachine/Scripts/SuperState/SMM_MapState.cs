
using System.Collections.Generic;
using DrawXXL;
using TGS;
using Unity.VisualScripting;
using UnityEngine;

public class SMM_MapState : GameplayBaseState
{
    

    public SMM_MapState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory)
    {
        IsRootState = true;

        Ctx.OnClickEvent += InputCellSelection;
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState() { }
    public override void ExitState() { }

    public override void CheckSwitchState()
    {
        
    }

    public override void InitializeSubState() 
    {
        Factory.MovementPhase();
    }



    public void InputCellSelection()
    {
        Cell cell = TGS.CellGetAtMousePosition();

        // If selected cell exists
        if (MapCtx.SelectedCell != null)
        {
            // Selected same cell
            if (MapCtx.SelectedCell == cell)
                return;

            if (MapCtx.MovementBorder != null)
                Destroy(MapCtx.MovementBorder);

            TGS.CellDestroyBorder(MapCtx.SelectedCell.index);
            MapCtx.SelectedCell = null;
        }

        if (cell == null)
            return;

        Debug.Log($"Cell Index: {cell.index}");
        MapCtx.SelectedCell = cell;

        TGS.CellDrawBorder(cell, Color.red, 1);


        // Check here for unit
        CheckForUnit();
    }

    public void CheckForUnit()
    {
        int cellIndex = MapCtx.SelectedCell.index;

        if (MapCtx.UnitExistOnCell(cellIndex))
        {
            List<Unit> units = MapCtx.GetUnitList(cellIndex);

            MapCtx.OnSelectUnit?.Invoke(units);
        }
    }
}
