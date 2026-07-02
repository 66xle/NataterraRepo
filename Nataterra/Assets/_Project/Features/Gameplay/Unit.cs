using System;
using UnityEngine;

public class Unit
{
    public string GUID { get; private set; }

    public int CellOrigin { get; private set; }
    public int CurrentMovement { get; private set; }


    public int Attack { get; private set; }
    public int Armour { get; private set; }
    public int Health { get; private set; }
    public int Initiative { get; private set; }
    public int Group { get; private set; }
    public int Movement { get; private set; }

    

    public Unit(UnitData data, int cellOrigin)
    {
        GUID = Guid.NewGuid().ToString();

        Attack = data.Attack;
        Armour = data.Armour;
        Health = data.Health;
        Initiative = data.Initiative;
        Group = data.Group;
        Movement = data.Movement;

        CellOrigin = cellOrigin;
        CurrentMovement = Movement;
    }
}
