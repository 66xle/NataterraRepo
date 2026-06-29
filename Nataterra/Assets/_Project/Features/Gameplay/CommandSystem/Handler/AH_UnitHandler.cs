using UnityEngine;

public class AH_UnitHandler : IActionHandler<AC_UnitSpawnCommand>
{
    GameplaySystem _gs;
    ServerMapWrapper _map;

    public AH_UnitHandler(GameplaySystem gs, ServerMapWrapper map)
    {
        _gs = gs;
        _map = map;
    }

    public void Handle(AC_UnitSpawnCommand command)
    {
        Debug.Log("Unit Spawned");
    }
}
