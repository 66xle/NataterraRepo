using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

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
        List<string> GUIDs = _map.AddUnit(units, cellIndex);

        _gs.UnitSystem.SpawnUnitToAll(units, GUIDs, cellIndex);
        _gs.SetLocalChanges(_map.GetStateChanges());
    }

    public void Handle(AC_UnitMoveCommand command)
    {
        List<Unit> selectedUnits = _map.GetUnits(command.SelectedIndex, command.ListOfUnitGUID, command.ListOfUnitType);

        if (selectedUnits == null || selectedUnits.Count != command.ListOfUnitGUID.Count)
            return;

        List<DijkstraResult> listOfResults = new();

        foreach (Unit unit in selectedUnits)
        {
            DijkstraResult result = _map.GetPathfindingResult(unit.GUID);

            if (result == null)
            {
                result = _gs.MSM.CalculateMovementRange(unit.CellOrigin, unit.Movement, _map.GetCells());
                _map.StoreResult(unit.GUID, result);
            }

            if (!result.Contains(command.Destination))
            {
                // Invalid destination
                return;
            }

            listOfResults.Add(result);
        }

        _map.MoveUnit(selectedUnits, command.SelectedIndex, command.Destination, listOfResults);

        _gs.UnitSystem.MoveUnit(command.ListOfUnitGUID, command.Destination);
        _gs.SetLocalChanges(_map.GetStateChanges());
    }
}
