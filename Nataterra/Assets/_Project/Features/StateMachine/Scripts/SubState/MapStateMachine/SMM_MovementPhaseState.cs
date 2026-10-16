using PurrNet;
using System.Collections.Generic;
using System.Linq;
using TGS;
using UnityEngine;

public class SMM_MovementPhaseState : GameplayBaseState
{
    List<Unit> _selectedUnits = new();

    public SMM_MovementPhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
    public override void EnterState()
    {
        Debug.Log("Entered Movement State");

        _selectedUnits.Clear();

        MapCtx.GS.UISystem.EnableEndPhaseButton(true);

        MapCtx.OnRequestEndPhase += RequestEndPhase;
        MapCtx.OnEndPhase += SwitchToResourcePhase;


        MapCtx.OnUnitSelect += UnitSelect;
        MapCtx.OnUnitDeselect += UnitDeselect;

        InputManager.Instance.OnRightClickEvent += MoveUnit;
    }
    public override void UpdateState() { }
    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        MapCtx.OnRequestEndPhase -= RequestEndPhase;
        MapCtx.OnEndPhase -= SwitchToResourcePhase;

        MapCtx.OnUnitSelect -= UnitSelect;
        MapCtx.OnUnitDeselect -= UnitDeselect;

        InputManager.Instance.OnRightClickEvent -= MoveUnit;
    }

    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }

    void SwitchToResourcePhase()
    {
        SwitchState(Factory.ResourcePhase());
    }

    void RequestEndPhase()
    {
        MapCtx.SendCommandToServer(CreateCommand.EndPhase());
    }

    void UnitSelect(Unit unit)
    {
        _selectedUnits.Add(unit);

        ShowUnitMovementRange();
    }

    void UnitDeselect(Unit unit)
    {
        _selectedUnits.Remove(unit);

        ShowUnitMovementRange();
    }

    private void ShowUnitMovementRange()
    {
        if (_selectedUnits.Count == 0) 
            return;

        int origin = MapCtx.GetOrigin(_selectedUnits, MapCtx.SelectedCell.index, out bool IsOrigin);

        int lowestMovement = MapCtx.GetLowestMovement(_selectedUnits, IsOrigin);

        if (lowestMovement == 0)
        {
            Debug.LogError($"MovementState: ShowUnitMovementRange: A selected unit has no movement avaliable");
            return;
        }

        // If all units are flying set to true
        bool flying = Extensions.EqualAll(_selectedUnits, true, units => units.IsFlying);

        DijkstraResult result = MapCtx.CalculateMovementRange(MapCtx.LocalPlayerID, origin, lowestMovement, TGS.cells, MapCtx.GetState(), flying);
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

        if (MapCtx.SelectedCell.index == cell.index)
            return;

        if (!MapCtx.MovementResult.Contains(cell.index))
        {
            // Display UI invalid destination
            Debug.LogError("MovementPhaseState: MoveUnit(): Invalid destination");
            return;
        }

        if (_selectedUnits.Count == 0)
        {
            Debug.LogError("MovementPhaseState: MoveUnit(): No units are selected");
            return;
        }

        MapCtx.SendCommandToServer(CreateCommand.UnitMove(MapCtx.SelectedCell.index, cell.index, _selectedUnits));

        MapCtx.MovementResult = null;
        MapCtx.RemoveMovementBorder();

        TGS.CellDestroyBorder(MapCtx.SelectedCell.index);
        MapCtx.SelectedCell = null;
    }
}
