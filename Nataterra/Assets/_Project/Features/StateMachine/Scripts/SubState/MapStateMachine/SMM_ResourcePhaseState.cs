using UnityEngine;

public class SMM_ResourcePhaseState : GameplayBaseState
{
    public SMM_ResourcePhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
    public override void EnterState()
    {
        Debug.Log("Entered Resource Phase");

        MapCtx.OnEndPhase += SwitchToCombatPhase;
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapCtx.SendCommandToServer(CreateCommand.EndPhase(GameplayState.ResourcePhase));
        }
    }

    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        MapCtx.OnEndPhase -= SwitchToCombatPhase;
    }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }

    private void SwitchToCombatPhase()
    {
        SwitchState(Factory.CombatPhase());
    }
}
