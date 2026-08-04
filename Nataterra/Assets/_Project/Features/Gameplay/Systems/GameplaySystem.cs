using PurrNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TGS;
using UnityEngine;

public class GameplaySystem : NetworkBehaviour
{
    public MapStateMachine MSM;

    public UnitSystem UnitSystem { get; private set; }
    public UISystem UISystem { get; private set; }

    public TerrainGridSystem TGS { get; private set; }


    private Queue<Action> _eventQueue = new();


    private void Awake()
    {
        UnitSystem = GetComponentInChildren<UnitSystem>();
        UISystem = GetComponentInChildren<UISystem>();
    }

    public void Setup()
    {
        TGS = TerrainGridSystem.instance;
    }

    public void AddEventQueue(Action action)
    {
        _eventQueue.Enqueue(action);

        if (_eventQueue.Count == 1)
        {
            _eventQueue.Peek()?.Invoke();
        }
    }

    public void RemoveEventQueue()
    {
        _eventQueue.Dequeue();

        if (_eventQueue.Count > 0)
        {
            _eventQueue.Peek()?.Invoke();
        }
    }



    [ObserversRpc]
    public void SetStateChanges(List<StateChange> changes)
    {
        foreach (StateChange state in changes)
        {
            MSM.SetCellState(state.State, state.CellIndex);
        }
    }

    [TargetRpc]
    public void SetClientFactionState(PlayerID playerID, FactionState state)
    {
        MSM.SetFactionState(state);
    }
}
