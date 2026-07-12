using UnityEngine;

public class SMM_WaitingForTurn : GameplayBaseState
{
    public SMM_WaitingForTurn(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory)
    {

    }
    public override void EnterState()
    {
        Debug.Log("Entered Waiting State");

        MapCtx.OnEndPhase += SwitchToMovementPhase;
    }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        MapCtx.OnEndPhase -= SwitchToMovementPhase;
    }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }

    void SwitchToMovementPhase()
    {
        SwitchState(Factory.MovementPhase());
    }
}
