
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
    }

    public override void EnterState()
    {
        InputManager.Instance.OnLeftClickEvent += InputCellSelection;

        InitializeSubState();
    }

    public override void UpdateState()
    {
        InputCellSelection();

        CheckSwitchState();
    }

    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        InputManager.Instance.OnLeftClickEvent -= InputCellSelection;
    }

    public override void CheckSwitchState()
    {
        
    }

    public override void InitializeSubState() 
    {
        SetSubState(Factory.MovementPhase());
        CurrentSubState.EnterState();
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
            {
                MapCtx.MovementResult = null;
                Destroy(MapCtx.MovementBorder);
            }

            // Selected cell
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
