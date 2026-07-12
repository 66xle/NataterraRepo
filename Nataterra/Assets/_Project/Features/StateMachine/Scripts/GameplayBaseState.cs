
using PurrNet;
using TGS;

public abstract class GameplayBaseState
{
    bool _isRootState = false;
    StateMachineManager _ctx;

    GameplayStateFactory _factory;
    GameplayBaseState _currentSuperState;
    GameplayBaseState _currentSubState;

    MapStateMachine _mapCtx;
    CombatStateMachine _combatCtx;

    TerrainGridSystem _tgs;

    public TerrainGridSystem TGS { get { return _tgs; } }

    public bool IsRootState { set { _isRootState = value; } }
    public StateMachineManager Ctx { get { return _ctx; } }
    public GameplayStateFactory Factory { get { return _factory; } }

    public MapStateMachine MapCtx { get {  return _mapCtx; } }
    public CombatStateMachine CombatCtx { get {  return _combatCtx; } }


    public GameplayBaseState CurrentSubState { get { return _currentSubState; } }


    public GameplayBaseState(StateMachineManager context, GameplayStateFactory factory)
    {
        _ctx = context;
        _factory = factory;

        _mapCtx = context.MapCtx;
        _combatCtx = context.CombatCtx;

        _tgs = TerrainGridSystem.instance;
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

        if (_isRootState)
            Ctx.CurrentState = newState;

        newState.EnterState();

        if (_isRootState)
        {
            // new root state, substate is not null
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
