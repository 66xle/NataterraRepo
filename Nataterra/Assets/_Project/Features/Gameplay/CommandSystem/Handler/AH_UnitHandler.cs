using UnityEngine;
using System.Collections.Generic;

public class AH_UnitHandler : IActionHandler<AC_UnitRecruitCommand>, IActionHandler<AC_UnitInitialSpawnCommand>, IActionHandler<AC_UnitMoveCommand>
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

    public void Handle(AC_UnitInitialSpawnCommand command)
    {
        int cellIndex = _map.GetBaseCellIndex(command.Faction);

        // Get Starting Unit
        List<UnitType> units = _map.FactionSettings[command.Faction].StartingUnits;
        List<GameObject> unitObjs = _map.AddUnit(units, cellIndex);

        _gs.UnitSystem.SpawnUnit(unitObjs, cellIndex, _map.GetStateChanges());
    }

    public void Handle(AC_UnitMoveCommand command)
    {
        List<Unit> selectedUnits = _map.GetUnits(command.SelectedIndex, command.ListOfUnitGUID, command.ListOfUnitType);

        if (selectedUnits == null)
            return;

        int origin = _gs.MSM.GetOrigin(selectedUnits, command.SelectedIndex);

        int lowestMovement = _gs.MSM.GetLowestMovement(selectedUnits);
        if (lowestMovement == 0)
        {
            // No movement message
            return;
        }

        DijkstraResult result = _gs.MSM.CalculateMovementRange(origin, lowestMovement, _map.GetCells());

        if (!result.Contains(command.Destination))
        {
            // Invalid destination
            return;
        }

        List<int> path = result.BuildPath(command.Destination);
        int cost = result.GetDestinationCost(command.Destination);

        _map.MoveUnit(selectedUnits, command.SelectedIndex, command.Destination, cost);

        

    }
}
