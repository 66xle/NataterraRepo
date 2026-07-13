using UnityEngine;

public class SMM_ResourcePhaseState : GameplayBaseState
{
    public SMM_ResourcePhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
    public override void EnterState()
    {
        Debug.Log("Entered Resource Phase");


    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapCtx.SendEndPhaseCommand(GameplayState.ResourcePhase);
        }
    }

    public override void FixedUpdateState() { }
    public override void ExitState() { }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }
}
