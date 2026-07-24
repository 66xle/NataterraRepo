using PurrNet;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using TGS;
using Unity.VisualScripting;
using UnityEngine;

public class ServerMap
{
    ServerMapWrapper _map;
    GameplaySystem _gs;

    private CommandProcessor _commandProcessor = new();

    public void Init(ServerMapWrapper wrapper, GameplaySystem GS)
    {
        _map = wrapper;
        _gs = GS;

        _commandProcessor.Register<AC_UnitRecruitCommand>(new AH_UnitHandler(_gs, _map));
        _commandProcessor.Register<AC_UnitInitialSpawnCommand>(new AH_UnitHandler(_gs, _map));
        _commandProcessor.Register<AC_UnitMoveCommand>(new AH_UnitHandler(_gs, _map));

        _commandProcessor.Register<AC_PhaseEndPhaseCommand>(new AH_PhaseHandler(_gs, _map));
    }

    public void HandleCommand(IActionCommand command)
    {
        _commandProcessor.Process(command);
    }

    public void LoadClientMap(PlayerID playerID)
    {
        List<string> GUIDs = new();
        List<UnitType> types = new();
        List<int> indexs = new();

        List<HexCellState> state = _map.GetState();

        for (int i = 0; i < state.Count; i++)
        {
            if (state[i].DictOfGroups.Count == 0) continue;

            foreach (UnitType type in state[i].DictOfGroups.Keys)
            {
                foreach (Unit unit in state[i].DictOfGroups[type].ListOfUnits)
                {
                    GUIDs.Add(unit.GUID);
                    types.Add(type);
                    indexs.Add(i);
                }
            }
        }

        _gs.UnitSystem.SpawnUnitToClient(playerID, types, GUIDs, indexs);

        if (_gs.networkManager.playerCount > 1)
            _gs.UISystem.ShowPhaseTitleClient(playerID, _map.GetGameplayState());

    }

    public List<HexCellState> GetMap()
    {
        return _map.GetState();
    }

    public FactionState GetFactionState(PlayerID playerID)
    {
        return _map.GetFactionState(playerID);
    }

    public void AddFaction(PlayerID playerID, Base facton)
    {
        _map.AddFaction(playerID, facton);
    }
}
