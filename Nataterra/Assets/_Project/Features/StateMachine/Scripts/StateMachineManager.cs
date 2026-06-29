using PurrNet;
using PurrNet.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TGS;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEditor.VersionControl.Asset;

public class StateMachineManager : NetworkBehaviour
{
    public MapStateMachine MapCtx;
    public CombatStateMachine CombatCtx;

    GameplayBaseState _currentState;
    GameplayStateFactory _states;

    public GameplayBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    List<Base> bases = new List<Base>{ Base.Beasts, Base.Velathi };
    Dictionary<PlayerID, Base> _dictFaction = new();

    protected override void OnSpawned()
    {
        base.OnSpawned();

        if (!isServer) return;

        networkManager.onPlayerJoined += OnPlayerJoined;

        Setup();
    }

    void Update()
    {
        if (_currentState == null) return;

        _currentState.UpdateStates();
    }

    void OnPlayerJoined(PlayerID player, bool isReconnect, bool asServer)
    {
        if (asServer) return;

        Base faction = bases[(int)player.id.value - 1];

        _dictFaction.Add(player, faction);

        Debug.Log($"Player {player.id}'s Faction: {faction}");
    }

    void Setup()
    {
        Debug.Log("Setup");

        MapCtx.Setup();

        _states = new GameplayStateFactory(this);
        _currentState = new SMM_MapState(this, _states);
    }
}
