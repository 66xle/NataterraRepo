using System.Collections.Generic;
using System.Linq;
using PurrNet;
using UnityEngine;
using UnityEngine.Analytics;

public class AH_PhaseHandler : IActionHandler<AC_PhaseEndPhaseCommand>
{
    GameplaySystem _gs;
    ServerMapWrapper _map;

    public AH_PhaseHandler(GameplaySystem gs, ServerMapWrapper map)
    {
        _gs = gs;
        _map = map;
    }

    public void Handle(AC_PhaseEndPhaseCommand command)
    {
        if (command.PlayerID != _map.CurrentPlayerTurn)
            return;

        GameplayState state = command.CurrentState;

        if (command.CurrentState == GameplayState.MovementPhase)
        {
            EndMovementPhase(command.PlayerID);
            state = GameplayState.ResourcePhase;
        }
        else if (command.CurrentState == GameplayState.ResourcePhase)
        {
            EndResourcePhase(command.PlayerID);
            state = GameplayState.CombatPhase;
        }
        else if (command.CurrentState == GameplayState.CombatPhase)
        {
            EndCombatPhase();
            state = GameplayState.DevelopmentPhase;
        }
        else if (command.CurrentState == GameplayState.DevelopmentPhase)
        {
            EndDevelopmentPhase(command.PlayerID ,command.Cart);
            state = GameplayState.WaitingForTurn;
        }

        // Set server's state
        _map.SetPhaseState(state);
        _gs.MSM.EndPhaseForClient(command.PlayerID);
    }

    void EndMovementPhase(PlayerID playerID)
    {
        Base faction = _map.GetFaction(playerID);

        List<HexCellState> state = _map.GetState();

        for (int i = 0; i < state.Count; i++)
        {
            _map.ResetUnitMovementOnCell(i);
        }

        _gs.SetStateChanges(_map.GetStateChanges());
    }

    void EndResourcePhase(PlayerID playerID)
    {
        Base faction = _map.GetFaction(playerID);
        UnitType type = _map.FactionSettings[faction].WorkerUnit;

        foreach (HexCellState state in _map.GetState())
        {
            if (!state.DictOfGroups.ContainsKey(type)) continue;

            _map.AddResource(faction, state.Resource);
        }

        _gs.SetClientFactionState(playerID, _map.GetFactionState(playerID));
    }

    void EndCombatPhase()
    {

    }

    void EndDevelopmentPhase(PlayerID playerID, DevelopmentCart cart)
    {
        // valdiate cost

        // spawn units
        Base faction = _map.GetFaction(playerID);
        int baseIndex = _map.GetBaseCellIndex(faction);

        List<UnitType> units = cart.Units.Keys.ToList();
        List<string> guids = _map.AddUnit(units, baseIndex);

        _gs.UnitSystem.SpawnUnitToAll(units, guids, baseIndex);
    }
}
