using PurrNet.Packing;
using System.Collections.Generic;
using UnityEngine;


public class Group : IPackedAuto
{
    public List<Unit> ListOfUnits;

    public Group(Unit unit)
    {
        ListOfUnits = new List<Unit>() { unit };
    }
}
