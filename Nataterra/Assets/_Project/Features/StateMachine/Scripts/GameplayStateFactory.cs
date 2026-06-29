
using PurrNet;
using System.Collections.Generic;

enum GameplayState
{
    Map,
    MovementPhase,
    ResourcePhase,
    CombatPhase,
    DevelopmentPhase,
    Combat,

}


public class GameplayStateFactory
{
    StateMachineManager _content;
    Dictionary<GameplayState, GameplayBaseState> _states = new Dictionary<GameplayState, GameplayBaseState>();

    public GameplayStateFactory(StateMachineManager currentContext)
    {
        _content = currentContext;
        _states[GameplayState.Map] = new SMM_MapState(_content, this);
        _states[GameplayState.Combat] = new SMC_CombatState(_content, this);
    }

    public GameplayBaseState Map()
    {
        return _states[GameplayState.Map];
    }

    public GameplayBaseState Combat()
    {
        return _states[GameplayState.Combat];
    }

    public GameplayBaseState MovementPhase()
    {
        return _states[GameplayState.MovementPhase];
    }

    public GameplayBaseState ResourcePhase()
    {
        return _states[GameplayState.ResourcePhase];
    }

    public GameplayBaseState CombatPhase()
    {
        return _states[GameplayState.CombatPhase];
    }

    public GameplayBaseState DevelopmentPhase()
    {
        return _states[GameplayState.DevelopmentPhase];
    }
}
