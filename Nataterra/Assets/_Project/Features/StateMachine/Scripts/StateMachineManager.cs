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
using static Unity.Cinemachine.CinemachineTriggerAction;
using static UnityEditor.VersionControl.Asset;

public class StateMachineManager : NetworkBehaviour
{
    public MapStateMachine MapCtx;
    public CombatStateMachine CombatCtx;

    GameplayBaseState _currentState;
    GameplayStateFactory _states;

    public GameplayBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

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
        if (!asServer) return;

        if (isReconnect)
        {
            Debug.Log($"Player {player.id} reconnected.");
            return;
        }

        List<FactionData> factions = GameManager.Instance.ListOfFactions;

        FactionData data = factions[(int)player.id.value - 1];

        _dictFaction.Add(player, data.Settings.Faction);

        Debug.Log($"Player {player.id}'s Faction: {data.Settings.Faction}");

        MapCtx.SetFactionSetting(data.Settings);
        MapCtx.SpawnStartingUnits(data.Settings.Faction);
    }

    void Setup()
    {
        Debug.Log("Setup");

        CreateDictDatabase(out Dictionary<UnitType, UnitData> dictUnits);

        MapCtx.Setup(dictUnits);

        SceneInitialize.Instance.Invoke();

        _states = new GameplayStateFactory(this);
        _currentState = new SMM_MapState(this, _states);
    }

    void CreateDictDatabase(out Dictionary<UnitType, UnitData> dictUnits)
    {
        Dictionary<UnitType, UnitData> units = new();

        List<FactionData> listOfFactions = GameManager.Instance.ListOfFactions;

        foreach (FactionData faction in listOfFactions)
        {
            foreach (UnitData unit in faction.ListOfUnits)
            {
                units.Add(unit.Unit, unit);
            }
        }

        dictUnits = units;
    }
}
