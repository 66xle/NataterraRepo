

using PurrNet;

public class SMM_MapState : GameplayBaseState
{
    public SMM_MapState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory)
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

    public override void InitializeSubState() 
    {
        Factory.MovementPhase();
    }
}
