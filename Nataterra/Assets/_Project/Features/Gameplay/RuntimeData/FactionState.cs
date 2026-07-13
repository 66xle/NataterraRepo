using System.Collections.Generic;

public class FactionState
{
    public int Food;
    public int Wood;
    public int Metal;

    public float FoodMultiplier;
    public float WoodMultiplier;
    public float MetalMultiplier;

    public List<UnitAmount> ListOfCurrentUnitAvaliable;

    public FactionState(List<UnitAmount> listUnitAvaliable)
    {
        Food = 3;
        Wood = 3;
        Metal = 3;

        FoodMultiplier = 1;
        WoodMultiplier = 1;
        MetalMultiplier = 1;

        ListOfCurrentUnitAvaliable = new List<UnitAmount>(listUnitAvaliable);
    }
}
