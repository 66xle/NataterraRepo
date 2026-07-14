using UnityEngine;

public class SMM_DevelopmentPhaseState : GameplayBaseState
{
    public SMM_DevelopmentPhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
    public override void EnterState()
    {
        Debug.Log("Entered Development Phase");

        MapCtx.OnEndPhase += SwitchToWaitingForTurn;
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapCtx.SendEndPhaseCommand(GameplayState.DevelopmentPhase);
        }
    }

    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        MapCtx.OnEndPhase -= SwitchToWaitingForTurn;
    }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }

    private void SwitchToWaitingForTurn()
    {
        SwitchState(Factory.WaitingForTurn());
    }
}
