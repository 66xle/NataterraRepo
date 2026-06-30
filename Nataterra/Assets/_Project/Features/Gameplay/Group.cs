using UnityEngine;

public class Group 
{
    public Unit Unit;
    public int Amount;

    public Group(Unit unit, int amount)
    {
        Unit = unit;
        Amount = amount;
    }
}
