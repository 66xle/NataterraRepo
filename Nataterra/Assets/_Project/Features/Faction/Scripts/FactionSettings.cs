using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class FactionSettings
{
    public Base Faction;
    public UnitType WorkerUnit;
    public List<UnitType> StartingUnits;
    public List<UnitAmount> ListOfUnitAvaliable;
}
