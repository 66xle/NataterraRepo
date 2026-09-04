using System.Collections.Generic;
using TGS;
using UnityEngine;
using VisualPath;
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
        Base faction = _map.GetFaction(command.PlayerID);
        int cellIndex = _map.GetBaseCellIndex(faction);

        // Get Starting Unit
        List<UnitType> units = _map.FactionSettings[faction].StartingUnits;
        List<string> GUIDs = _map.AddUnit(command.PlayerID, units, cellIndex);

        _gs.UnitSystem.SpawnUnitToAll(units, GUIDs, cellIndex);
        _gs.SetStateChanges(_map.GetStateChanges());
    }

    public void Handle(AC_UnitMoveCommand command)
    {
        if (command.PlayerID != _map.CurrentPlayerTurn)
            return;

        List<Unit> selectedUnits = _map.GetUnits(command.PlayerID, command.SelectedIndex, command.ListOfUnitGUID, command.ListOfUnitType);

        if (selectedUnits == null || selectedUnits.Count != command.ListOfUnitGUID.Count)
            return;

        // Used to determine each unit's movement cost
        List<DijkstraResult> listOfResults = new();
        foreach (Unit unit in selectedUnits)
        {
            DijkstraResult result = _map.GetPathfindingResult(unit.GUID);

            if (result == null)
            {
                result = _gs.MSM.CalculateMovementRange(command.PlayerID, unit.CellOrigin, unit.Movement, _map.GetCells(), _map.GetState(), unit.IsFlying);
                _map.StoreResult(unit.GUID, result);
            }

            if (!result.Contains(command.Destination))
            {
                Debug.LogError($"UnitHandler: Handle: Destination is not in range for Unit {unit.UnitType}");
                return;
            }

            listOfResults.Add(result);
        }

        // Used to calculate current unit's pathing
        int origin = _gs.MSM.GetOrigin(selectedUnits, command.SelectedIndex, out bool IsOrigin);
        int lowestMovement = _gs.MSM.GetLowestMovement(selectedUnits, IsOrigin);
        bool flying = Extensions.EqualAll(selectedUnits, true, units => units.IsFlying);
        DijkstraResult groupResult = _gs.MSM.CalculateMovementRange(command.PlayerID, origin, lowestMovement, _map.GetCells(), _map.GetState(), flying);

        List<int> path = _gs.MSM.FindShortestPathWithinMovementRange(command.SelectedIndex, command.Destination, _map.GetCells(), _map.GetState(), groupResult, command.PlayerID, flying);



        // Save unit's new position
        _map.MoveUnit(command.PlayerID, selectedUnits, command.SelectedIndex, command.Destination, listOfResults);


        //List<int> path = listOfResults[0].BuildPath(command.Destination);
        List<Cell> cellPath = _map.GetCellPath(path);
        List<VisualHex> listHexes = _gs.PathSystem.ConvertHexes(cellPath);

        Vector3 start = _gs.TGS.CellGetPosition(path[0]);
        Vector3 end = _gs.TGS.CellGetPosition(path[path.Count - 1]);

        _gs.PathSystem.GeneratePath(listHexes, start, end);
        _gs.UnitSystem.MoveUnit(command.ListOfUnitGUID, command.Destination);
        _gs.SetStateChanges(_map.GetStateChanges());
    }
}
