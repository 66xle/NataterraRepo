using PurrNet;
using UnityEngine;
using System.Collections.Generic;
using TGS;
using System.Windows.Input;
using System;


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
    }

    public void HandleCommand(IActionCommand command)
    {
        _commandProcessor.Process(command);
    }
    public void SetFactionSetting(FactionSettings settings)
    {
        _map.SetFactionSettings(settings);
    }
}
