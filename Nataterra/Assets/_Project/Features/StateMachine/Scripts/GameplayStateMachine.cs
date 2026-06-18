using UnityEngine;

public class GameplayStateMachine : MonoBehaviour
{

    GameplayBaseState _currentState;
    GameplayStateFactory _states;

    public GameplayBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    void Update()
    {
        if (_currentState == null) return;

        _currentState.UpdateStates();
    }
}
