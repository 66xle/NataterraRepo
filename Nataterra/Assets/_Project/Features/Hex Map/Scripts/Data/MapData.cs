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
    public int[] bases;

    public int row;
    public int column;

    public MapData(HexCell[] cells, List<Cell> tgsCells, string sceneName, int[] bases, int row, int column)
    {
        this.hexCells = cells.Select(c => new HexCellData(c)).ToList();
        this.tgsCells = tgsCells.Select(c => new CellData(c)).ToList();
        this.sceneName = sceneName;
        this.bases = bases;

        this.row = row;
        this.column = column;
    }
}
