using UnityEngine;
using System.Collections.Generic;

public class AH_UnitHandler : IActionHandler<AC_UnitRecruitCommand>, IActionHandler<AC_InitialUnitSpawnCommand>
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
        _map.AddUnit(command.Unit, cellIndex);
       

        // send data to clients map
        _gs.SetLocalChanges(_map.GetStateChanges());  

        //_gs.UnitSystem.SpawnUnit(command.Unit, cellIndex);


        Debug.Log("Unit Spawned");
    }

    public void Handle(AC_InitialUnitSpawnCommand command)
    {
        int cellIndex = _map.GetBaseCellIndex(command.Faction);

        // Get Starting Unit
        List<UnitType> units = _map.FactionSetting.StartingUnits;
        List<GameObject> unitObjs = _map.AddUnit(units, cellIndex);

        _gs.UnitSystem.SpawnUnit(unitObjs, cellIndex, _map.GetStateChanges());
    }
}
