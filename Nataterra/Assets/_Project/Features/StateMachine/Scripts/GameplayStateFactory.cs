
using System.Collections.Generic;

enum GameplayState
{
    Map,
    Combat
}


public class GameplayStateFactory
{
    StateMachineManager _content;
    Dictionary<GameplayState, GameplayBaseState> _states = new Dictionary<GameplayState, GameplayBaseState>();

    public GameplayStateFactory(StateMachineManager currentContext)
    {
        _content = currentContext;
        _states[GameplayState.Map] = new MapState(_content, this);
        _states[GameplayState.Combat] = new CombatState(_content, this);
    }

    public GameplayBaseState Map()
    {
        return _states[GameplayState.Map];
    }

    public GameplayBaseState Combat()
    {
        return _states[GameplayState.Combat];
    }

}
