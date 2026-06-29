using UnityEngine;

public struct AC_UnitSpawnCommand : IActionCommand
{
    public Base Faction;
    public int Amount;
    public Unit Unit;
}
