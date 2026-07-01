using System;
using UnityEngine;

public class Unit
{
    public string GUID;


    public Unit(UnitData data)
    {
        GUID = Guid.NewGuid().ToString();
    }
}
