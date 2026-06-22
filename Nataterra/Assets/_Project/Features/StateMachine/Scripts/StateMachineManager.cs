using System.Collections;
using System.Collections.Generic;
using TGS;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    public MapStateMachine MapCtx;
    public CombatStateMachine CombatCtx;

    GameplayBaseState _currentState;
    GameplayStateFactory _states;

    public GameplayBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    


    void Start()
    {
        
    }

    void Update()
    {
        if (_currentState == null) return;

        _currentState.UpdateStates();
    }

}
