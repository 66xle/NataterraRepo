using System;
using UnityEngine;

[Serializable]
public struct UnitAmount
{
    public UnitType Type;
    public int Amount;

    public UnitAmount(UnitType type, int amount)
    {
        Type = type;
        Amount = amount;
    }
}
