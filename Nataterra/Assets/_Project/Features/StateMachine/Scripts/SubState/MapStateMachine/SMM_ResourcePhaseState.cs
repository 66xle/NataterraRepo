using UnityEngine;

public class SMM_ResourcePhaseState : GameplayBaseState
{
    public SMM_ResourcePhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
    public override void EnterState()
    {
        Debug.Log("Entered Resource Phase");

        MapCtx.GS.UISystem.EnableEndPhaseButton(true);

        MapCtx.OnRequestEndPhase += RequestEndPhase;
        MapCtx.OnEndPhase += SwitchToCombatPhase;
    }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        MapCtx.OnRequestEndPhase -= RequestEndPhase;
        MapCtx.OnEndPhase -= SwitchToCombatPhase;
    }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }

    private void SwitchToCombatPhase()
    {
        SwitchState(Factory.CombatPhase());
    }

    void RequestEndPhase()
    {
        MapCtx.SendCommandToServer(CreateCommand.EndPhase());
    }
}
