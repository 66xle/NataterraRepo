
public abstract class GameplayBaseState
{
    bool isRootState = false;
    GameplayStateMachine _ctx;
    GameplayStateFactory _factory;
    GameplayBaseState _currentSuperState;
    GameplayBaseState _currentSubState; // CHANGE TO PROTECTED LATER

    public GameplayStateMachine Ctx { get; set; }
    public GameplayStateMachine Factory { get; set; }

    public GameplayBaseState(GameplayStateMachine context, GameplayStateFactory factory)
    {
        _ctx = context;
        _factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateState();
        }
    }

    public void FixedUpdateStates()
    {
        FixedUpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.FixedUpdateState();
        }
    }

    protected void SwitchState(GameplayBaseState newState)
    {
        ExitState();

        if (isRootState)
            Ctx.CurrentState = newState;

        newState.EnterState();

        if (isRootState)
        {
            // new root state, substate is null
            if (_currentSubState != null && newState._currentSubState != null)
            {
                // Exit roots substates
                if (_currentSubState.ToString() != newState._currentSubState.ToString())
                {
                    _currentSubState.ExitState();
                    _currentSubState = newState._currentSubState;
                }
            }


        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }
    protected void SetSuperState(GameplayBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(GameplayBaseState newSubState)
    {
        // Sets the supers sub state
        _currentSubState = newSubState;

        // Set the sub's super state
        newSubState.SetSuperState(this);
    }
}
