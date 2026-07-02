

using DrawXXL;
using TGS;
using Unity.VisualScripting;
using UnityEngine;

public class SMM_MapState : GameplayBaseState
{
    Cell _selectedCell;

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
        if (_selectedCell != null)
        {
            // Selected same cell
            if (_selectedCell == cell)
                return;

            TGS.CellDestroyBorder(_selectedCell.index);
            _selectedCell = null;
            return;
        }

        if (cell == null)
            return;

        Debug.Log($"Cell Index: {cell.index}");
        _selectedCell = cell;

        TGS.CellDrawBorder(cell, Color.red, 1);
    }
}
