using UnityEngine;

public class SMM_DevelopmentPhaseState : GameplayBaseState
{
    public SMM_DevelopmentPhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapCtx.SendEndPhaseCommand(GameplayState.DevelopmentPhase);
        }
    }

    public override void FixedUpdateState() { }
    public override void ExitState() { }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }
}
