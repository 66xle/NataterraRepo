using PurrNet;
using UnityEngine;

public class CombatPhaseState : GameplayBaseState
{
    public CombatPhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
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
}
