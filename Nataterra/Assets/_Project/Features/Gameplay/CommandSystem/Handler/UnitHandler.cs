using UnityEngine;

public class UnitHandler : IActionCommandHandler<UnitSpawnCommand>
{
    GameplaySystem _gs;
    ServerMapWrapper _map;

    public UnitHandler(GameplaySystem gs, ServerMapWrapper map)
    {
        _gs = gs;
        _map = map;
    }

    public void Handle(UnitSpawnCommand command)
    {
        Debug.Log("Unit Spawned");
    }
}
