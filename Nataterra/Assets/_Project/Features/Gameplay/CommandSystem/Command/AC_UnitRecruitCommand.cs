using PurrNet;
using UnityEngine;

public struct AC_UnitRecruitCommand : IActionCommand
{
    public Base Faction;
    public UnitType Unit;
    public int Amount;
}
