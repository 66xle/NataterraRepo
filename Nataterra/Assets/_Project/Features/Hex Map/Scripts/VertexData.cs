using System.Collections.Generic;
using TGS;
using UnityEngine;

public class VertexData
{
    public Vector3 position;
    public List<(Cell, int)> cellsRef = new List<(Cell, int)>();

    public VertexData(Vector3 position, Cell cell, int vertexIndex)
    {
        this.position = position;
        AddCellIndex(cell, vertexIndex);
    }

    public void AddCellIndex(Cell cell, int vertexIndex)
    {
        cellsRef.Add((cell, vertexIndex));
    }
}
