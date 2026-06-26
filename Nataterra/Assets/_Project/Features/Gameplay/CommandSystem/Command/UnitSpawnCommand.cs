using UnityEngine;

public struct UnitSpawnCommand : IActionCommand
{
    public Base Faction;
    public int Amount;
    public Unit Unit;
}
