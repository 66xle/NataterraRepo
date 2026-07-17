using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async void Handle(AC_PhaseEndPhaseCommand command)
    {
        if (command.PlayerID != _map.CurrentPlayerTurn)
            return;

        // Validate current state

        bool error = false;

        GameplayState state = command.CurrentState;

        if (command.CurrentState == GameplayState.WaitingForTurn)
        {
            _gs.UISystem.ShowFactionTurn(_map.GetFaction(command.PlayerID));

            await Task.Delay(1500);

            state = GameplayState.MovementPhase;
        }
        else if (command.CurrentState == GameplayState.MovementPhase)
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
            error = EndDevelopmentPhase(command.PlayerID ,command.Cart);
            state = GameplayState.WaitingForTurn;
        }

        if (error) return;

        _gs.MSM.EndPhaseForClient(command.PlayerID);

        if (state != GameplayState.WaitingForTurn)
        {
            // Set server's state
            _map.SetPhaseState(state);

            _gs.UISystem.ShowPhaseTitle(state);
            return;
        }

        NextPlayer();
    }

    async void NextPlayer()
    {
        // Next player
        _map.NextPlayerTurn();

        PlayerID nextPlayer = _map.CurrentPlayerTurn;

        _gs.UISystem.ShowFactionTurn(_map.GetFaction(nextPlayer));

        await Task.Delay(1500);

        _map.SetPhaseState(GameplayState.MovementPhase);

        _gs.UISystem.ShowPhaseTitle(GameplayState.MovementPhase);
        _gs.MSM.EndPhaseForClient(nextPlayer);
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
        _gs.UISystem.ResourceUpdateClientUI(playerID);
    }

    void EndCombatPhase()
    {

    }

    bool EndDevelopmentPhase(PlayerID playerID, DevelopmentCart cart)
    {
        Base faction = _map.GetFaction(playerID);
        

        // Check units avaliable
        if (!_map.IsUnitsAvaliable(faction, cart.Units))
        {
            Debug.Log($"PhaseHandler: EndDevelopmentPhase(): Not enough units");
            return true;
        }

        // valdiate cost
        if (!_map.ReduceResources(faction, cart.Units))
        {
            Debug.Log($"PhaseHandler: EndDevelopmentPhase(): Not enough resources");
            return true;
        }

        // spawn units
        int baseIndex = _map.GetBaseCellIndex(faction);

        List<UnitType> units = cart.GetUnitsInList();
        List<string> guids = _map.AddUnit(units, baseIndex);


        _gs.SetClientFactionState(playerID, _map.GetFactionState(playerID));
        _gs.UISystem.ResourceUpdateClientUI(playerID);
        _gs.UISystem.UnitAvaliableUpdateClientUI(playerID, units);

        _gs.UnitSystem.SpawnUnitToAll(units, guids, baseIndex);

        return false;
    }

}
