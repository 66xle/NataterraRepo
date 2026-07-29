using System.Collections.Generic;

public class FactionState
{
    public int Food;
    public int Wood;
    public int Metal;

    public int FoodMultiplier;
    public int WoodMultiplier;
    public int MetalMultiplier;

    public Dictionary<UnitType, int> CurrentUnitAvaliable;

    public FactionState(List<UnitData> units)
    {
        Food = 10;
        Wood = 10;
        Metal = 10;

        FoodMultiplier = 1;
        WoodMultiplier = 1;
        MetalMultiplier = 1;

        CurrentUnitAvaliable = new();

        foreach (UnitData unit in units)
        {
            CurrentUnitAvaliable.Add(unit.UnitType, unit.StartingAvailiableUnits);
        }
    }
}
