using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Faction", menuName = "Scriptable/Faction Data", order = 1)]
public class FactionData : ScriptableObject
{
    public FactionSettings Settings;
    public List<UnitData> ListOfUnits;
}
