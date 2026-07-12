using UnityEngine;

public class SMC_CombatState : GameplayBaseState
{
    public SMC_CombatState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory)
    {
        IsRootState = true;
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
    public override void InitializeSubState() { }
}
