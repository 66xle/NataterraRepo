using PurrNet;
using PurrNet.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TGS;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static Unity.Cinemachine.CinemachineTriggerAction;
using static UnityEditor.VersionControl.Asset;

public class StateMachineManager : NetworkBehaviour
{
#if UNITY_EDITOR
    [Header("Debugging")]
    public TMP_Text CellLabelPrefab;
    public Canvas GridCanvas;
    public bool ShowCellIndex = false;
#endif

    [Header("References")]
    public MapStateMachine MapCtx;
    public CombatStateMachine CombatCtx;

    GameplayBaseState _currentState;
    GameplayStateFactory _states;

    public GameplayBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    protected override async void OnSpawned(bool asServer)
    {
        base.OnSpawned();

        if (!asServer)
        {
            await MapCtx.SetupClient(CreateDictDatabase());

            SetupStateMachine();
            OnPlayerJoin();
            return;
        }

        Setup();
    }

    void Update()
    {
        if (_currentState == null) return;

        _currentState.UpdateStates();
    }

    
    void SetupStateMachine()
    {
        _states = new GameplayStateFactory(this);
        _currentState = new SMM_MapState(this, _states);
        _currentState.EnterState();
    }

    void OnPlayerJoin()
    {
        MapCtx.SpawnStartingUnits();

        if (networkManager.playerCount == 1)
        {
            MapCtx.SendEndPhaseCommand(GameplayState.WaitingForTurn);
        }
    }

    void OnPlayerJoined(PlayerID player, bool isReconnect, bool asServer)
    {
        if (asServer) return;

        if (isReconnect)
        {
            Debug.Log($"Player {player.id} reconnected.");
            return;
        }

        List<FactionData> factions = GameManager.Instance.ListOfFactions;
        FactionData data = factions[(int)player.id.value - 1];

        //_dictFaction.Add(player, data.Settings.Faction);

        Debug.Log($"Player {player.id}'s Faction: {data.Settings.Faction}");

        MapCtx.SpawnStartingUnits();
    }

    void Setup()
    { 
        Debug.Log("Setup");

        List<HexCellState> tempState = new();

        MapData mapData = GameManager.Instance.MapData;
        foreach (HexCellData data in mapData.hexCells)
        {
            tempState.Add(new HexCellState(data));
        }

        MapCtx.SetupServer(CreateDictDatabase(), tempState);

#if UNITY_EDITOR
        if (ShowCellIndex)
        {
            CreateCellLabel();
        }
#endif
    }

    Dictionary<UnitType, UnitData> CreateDictDatabase()
    {
        Dictionary<UnitType, UnitData> units = new();

        List<FactionData> listOfFactions = GameManager.Instance.ListOfFactions;

        foreach (FactionData faction in listOfFactions)
        {
            foreach (UnitData unit in faction.ListOfUnits)
            {
                units.Add(unit.UnitType, unit);
            }
        }

        return units;
    }

#if UNITY_EDITOR

    void CreateCellLabel()
    {
        for (int i = 0; i < TerrainGridSystem.instance.cellCount; i++)
        {
            int cellIndex = TerrainGridSystem.instance.cells[i].index;
            Vector3 position = TerrainGridSystem.instance.CellGetPosition(cellIndex);

            TMP_Text label = Instantiate<TMP_Text>(CellLabelPrefab);
            label.rectTransform.SetParent(GridCanvas.transform, false);
            label.rectTransform.position = position;

            label.rectTransform.localScale = new Vector3(label.rectTransform.localScale.x * TerrainGridSystem.instance.cellSize.x, label.rectTransform.localScale.z * TerrainGridSystem.instance.cellSize.y, 1f);
            label.text = cellIndex.ToString();
        }
    }
#endif
}
