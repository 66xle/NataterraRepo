using System.Collections.Generic;
using UnityEngine;

public struct DevelopmentCart
{
    public Dictionary<UnitType, int> Units;

    /// <summary>
    /// Returns a list of UnitTypes
    /// </summary>
    /// <returns></returns>
    public List<UnitType> GetUnitsInList()
    {
        List<UnitType> types = new();

        foreach (UnitType unitType in Units.Keys)
        { 
            for (int i = 0; i < Units[unitType]; i++)
            {
                types.Add(unitType);
            }
        }

        return types;
    }
}
