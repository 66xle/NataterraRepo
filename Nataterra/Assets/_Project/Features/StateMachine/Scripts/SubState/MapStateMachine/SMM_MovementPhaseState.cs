using System.Collections.Generic;
using TGS;
using UnityEngine;

public class SMM_MovementPhaseState : GameplayBaseState
{
    public SMM_MovementPhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) 
    {
        MapCtx.OnSelectUnit += ShowUnitMovementRange;
    }
    public override void EnterState()
    {
        
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

    public override void InitializeSubState() { }

    private void ShowUnitMovementRange(List<Unit> units)
    {
        Unit unitOrigin = units[0];

        for (int i = 1; i < units.Count; i++)
        {
            if (units[i].CurrentMovement < unitOrigin.CurrentMovement)
                unitOrigin = units[i];
        }

        List<int> cellsInRange = TGS.CellGetNeighboursWithinRangeHex(unitOrigin.CellOrigin, 1, unitOrigin.CurrentMovement);

        Debug.Log($"Cell In Range: {cellsInRange.Count}");

        if (MapCtx.MovementBorder != null)
            Destroy(MapCtx.MovementBorder);

        MapCtx.MovementBorder = TGS.CellDrawBorder(cellsInRange, Color.green);
    }
}
