using System.Collections.Generic;
using UnityEngine;


public class Group 
{
    public List<Unit> ListOfUnits;

    public Group(Unit unit)
    {
        ListOfUnits = new List<Unit>() { unit };
    }
}
