using PurrNet;
using UnityEngine;

public class SMM_CombatPhaseState : GameplayBaseState
{
    public SMM_CombatPhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
    public override void EnterState()
    {
        Debug.Log("Entered Combat Phase");

        MapCtx.OnEndPhase += SwitchToDevelopmentPhase;

    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapCtx.SendEndPhaseCommand(GameplayState.CombatPhase);
        }
    }

    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        MapCtx.OnEndPhase -= SwitchToDevelopmentPhase;
    }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }

    private void SwitchToDevelopmentPhase()
    {
        SwitchState(Factory.DevelopmentPhase());
    }
}
