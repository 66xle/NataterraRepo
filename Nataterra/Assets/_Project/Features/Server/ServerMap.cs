using PurrNet;
using UnityEngine;
using System.Collections.Generic;
using TGS;
using System.Windows.Input;



public class ServerMap : NetworkBehaviour
{
    ServerMapWrapper _map;
    GameplaySystem _gs;

    private CommandProcessor _commandProcessor = new();

    private void OnEnable()
    {
        InstanceHandler.RegisterInstance(this);

        _commandProcessor.Register<AC_UnitSpawnCommand>(new AH_UnitHandler(_gs, _map));
    }

    private void OnDisable()
    {
        InstanceHandler.UnregisterInstance<ServerMap>();
    }

    public void Init(MapData mapData, GameplaySystem GS)
    {
        _gs = GS;
        _map = new ServerMapWrapper(mapData.hexCells, mapData.bases);
    }

    public void HandleCommand(IActionCommand command)
    {
        _commandProcessor.Process(command);
    }

}
