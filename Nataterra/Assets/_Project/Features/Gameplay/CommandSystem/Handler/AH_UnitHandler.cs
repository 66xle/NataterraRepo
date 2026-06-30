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


        int cellIndex = _map.GetBaseCellIndex(command.Faction);

        // server update map
        _map.AddUnit(command.Unit, command.Amount, cellIndex);
       

        // send data to clients map
        _gs.SetLocalChanges(_map.GetStateChanges());  

        _gs.UnitSystem.RecruitUnit(command.Amount, command.Unit, cellIndex);


        Debug.Log("Unit Spawned");
    }
}
