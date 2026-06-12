using System.Collections.Generic;
using UnityEngine;
using TGS;

public class MapData : ScriptableObject
{
    public HexCell[] cells;
    public List<Cell> tgsCells;

    // scene or prefab reference here

    public MapData(HexCell[] cells, List<Cell> tgsCells)
    {
        this.cells = cells;
        this.tgsCells = tgsCells;
    }
}
