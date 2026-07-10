using UnityEngine;
using System.Collections.Generic;
using TGS;
using System.Windows.Input;
using System;
using PurrNet;

public class ServerMap : NetworkBehaviour
{
    ServerMapWrapper _map;
    GameplaySystem _gs;

    private CommandProcessor _commandProcessor = new();

    private void OnEnable()
    {
        InstanceHandler.RegisterInstance(this);
    }

    private void OnDisable()
    {
        InstanceHandler.UnregisterInstance<ServerMap>();
    }

    public void Init(ServerMapWrapper wrapper, GameplaySystem GS)
    {
        _map = wrapper;
        _gs = GS;

        _commandProcessor.Register<AC_UnitRecruitCommand>(new AH_UnitHandler(_gs, _map));
        _commandProcessor.Register<AC_UnitInitialSpawnCommand>(new AH_UnitHandler(_gs, _map));
        _commandProcessor.Register<AC_UnitMoveCommand>(new AH_UnitHandler(_gs, _map));
    }

    public void HandleCommand(IActionCommand command)
    {
        _commandProcessor.Process(command);
    }

    public List<HexCellState> LoadClientMap(PlayerID playerID)
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

        return state;
    }
}
