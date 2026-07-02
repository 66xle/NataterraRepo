using System;
using UnityEngine;

public class Unit
{
    public string GUID;

    public int Attack;
    public int Armour;
    public int Health;
    public int Initiative;
    public int Group;
    public int Movement;

    public Unit(UnitData data)
    {
        GUID = Guid.NewGuid().ToString();

        Attack = data.Attack;
        Armour = data.Armour;
        Health = data.Health;
        Initiative = data.Initiative;
        Group = data.Group;
        Movement = data.Movement;
    }
}
