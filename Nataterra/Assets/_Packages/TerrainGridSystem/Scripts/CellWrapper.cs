using System;
using System.Collections.Generic;
using TGS;
using UnityEngine;


public class CellWrapper
{
    public Vector2 center;
    public List<Vector2> points;

    public int index;
    public bool canCross;

    public CellWrapper(Vector2 center, List<Vector2> points, int index, bool canCross)
    {
        this.center = center;
        this.points = points;
        this.index = index;
        this.canCross = canCross;
    }
}
