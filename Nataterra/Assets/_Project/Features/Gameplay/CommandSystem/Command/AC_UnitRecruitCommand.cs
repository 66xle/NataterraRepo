using PurrNet;
using UnityEngine;

public struct AC_UnitRecruitCommand : IActionCommand
{
    public Base Faction;
    public Unit Unit;
    public int Amount;
}
