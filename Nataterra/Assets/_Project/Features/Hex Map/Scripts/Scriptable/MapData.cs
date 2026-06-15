using System.Collections.Generic;
using UnityEngine;
using TGS;
using System.Linq;
using System;

[Serializable]
public class MapData : ScriptableObject
{
    public List<HexCellData> hexCells;
    public List<Cell> tgsCells;

    // scene or prefab reference here

    public void Initialize(HexCell[] cells, List<Cell> tgsCells)
    {
        this.hexCells = cells.Select(c => new HexCellData(c)).ToList();
        this.tgsCells = tgsCells;
    }
}
