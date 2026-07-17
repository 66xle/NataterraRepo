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

    public FactionState(List<UnitAmount> listUnitAvaliable)
    {
        Food = 10;
        Wood = 10;
        Metal = 10;

        FoodMultiplier = 1;
        WoodMultiplier = 1;
        MetalMultiplier = 1;

        CurrentUnitAvaliable = new();

        foreach (UnitAmount unit in listUnitAvaliable)
        {
            CurrentUnitAvaliable.Add(unit.Type, unit.Amount);
        }
    }
}
