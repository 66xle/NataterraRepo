using System.Collections.Generic;
using UnityEngine;
using TGS;
using System.Linq;
using System;

[Serializable]
public class MapData
{
    public List<HexCellData> hexCells;
    public List<CellData> tgsCells;
    public string sceneName;

    // scene or prefab reference here

    public MapData(HexCell[] cells, List<Cell> tgsCells, string sceneName)
    {
        this.hexCells = cells.Select(c => new HexCellData(c)).ToList();
        this.tgsCells = tgsCells.Select(c => new CellData(c)).ToList();
        this.sceneName = sceneName;
    }
}
