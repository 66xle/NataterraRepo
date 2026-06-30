using UnityEngine;

public class AH_UnitHandler : IActionHandler<AC_UnitRecruitCommand>
{
    GameplaySystem _gs;
    ServerMapWrapper _map;

    public AH_UnitHandler(GameplaySystem gs, ServerMapWrapper map)
    {
        _gs = gs;
        _map = map;
    }

    public void Handle(AC_UnitRecruitCommand command)
    {
        // validate server map against client map


        // server update map
        // send data to clients map


        _gs.UnitSystem.RecruitUnit(command.Amount, command.Unit, _map.GetBaseCellIndex(command.Faction));


        Debug.Log("Unit Spawned");
    }
}
