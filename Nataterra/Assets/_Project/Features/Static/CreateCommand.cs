using PurrNet;
using System.Collections.Generic;
using UnityEngine;

public static class CreateCommand 
{
    public static AC_UnitInitialSpawnCommand UnitInitialSpawn()
    {
        AC_UnitInitialSpawnCommand command = new();

        return command;
    }

    public static AC_UnitMoveCommand? UnitMove(int origin, int destination, List<Unit> selectedUnits)
    {
        List<string> guids = new();
        List<UnitType> type = new();

        if (selectedUnits.Count == 0)
        {
            Debug.LogError("CreateCommand: UnitMove() - No units selected!");
            return null;
        }

        foreach (Unit unit in selectedUnits)
        {
            guids.Add(unit.GUID);
            type.Add(unit.UnitType);
        }

        // Send Command
        AC_UnitMoveCommand command = new AC_UnitMoveCommand()
        {
            ListOfUnitType = type,
            ListOfUnitGUID = guids,
            SelectedIndex = origin,
            Destination = destination
        };

        return command;
    }

    public static AC_PhaseEndPhaseCommand EndPhase(GameplayState state)
    {
        AC_PhaseEndPhaseCommand command = new AC_PhaseEndPhaseCommand()
        {
            CurrentState = state
        };

        return command;
    }
}
