using System;
using System.Collections.Generic;
using TGS;
using UnityEngine;



[Serializable]
public class CellData
{
    public Vector2 center;
    public List<Vector2> points;

    public int index;

    public CellData(Cell data)
    {
        center = data.center;
        points = data.region.points;

        index = data.index;
    }
}
