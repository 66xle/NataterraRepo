using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class AH_UnitHandler : IActionHandler<AC_UnitInitialSpawnCommand>, IActionHandler<AC_UnitMoveCommand>
{
    GameplaySystem _gs;
    ServerMapWrapper _map;

    public AH_UnitHandler(GameplaySystem gs, ServerMapWrapper map)
    {
        _gs = gs;
        _map = map;
    }

    public void Handle(AC_UnitInitialSpawnCommand command)
    {
        Base faction = _map.GetFaction(command.ID);
        int cellIndex = _map.GetBaseCellIndex(faction);

        // Get Starting Unit
        List<UnitType> units = _map.FactionSettings[faction].StartingUnits;
        List<string> GUIDs = _map.AddUnit(command.ID, units, cellIndex);

        _gs.UnitSystem.SpawnUnitToAll(units, GUIDs, cellIndex);
        _gs.SetStateChanges(_map.GetStateChanges());
    }

    public void Handle(AC_UnitMoveCommand command)
    {
        if (command.ID != _map.CurrentPlayerTurn)
            return;

        List<Unit> selectedUnits = _map.GetUnits(command.ID, command.SelectedIndex, command.ListOfUnitGUID, command.ListOfUnitType);

        if (selectedUnits == null || selectedUnits.Count != command.ListOfUnitGUID.Count)
            return;

        List<DijkstraResult> listOfResults = new();

        foreach (Unit unit in selectedUnits)
        {
            DijkstraResult result = _map.GetPathfindingResult(unit.GUID);

            if (result == null)
            {
                result = _gs.MSM.CalculateMovementRange(command.ID, unit.CellOrigin, unit.Movement, _map.GetCells(), _map.GetState(), unit.IsFlying);
                _map.StoreResult(unit.GUID, result);
            }

            if (!result.Contains(command.Destination))
            {
                Debug.LogError($"UnitHandler: Handle: Destination is not in range for Unit {unit.UnitType}");
                return;
            }

            listOfResults.Add(result);
        }

        _map.MoveUnit(command.ID, selectedUnits, command.SelectedIndex, command.Destination, listOfResults);

        _gs.UnitSystem.MoveUnit(command.ListOfUnitGUID, command.Destination);
        _gs.SetStateChanges(_map.GetStateChanges());
    }
}
