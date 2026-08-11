using PurrNet;
using System.Collections.Generic;
using TGS;
using UnityEngine;
using VisualPath;

public class PathSystem : NetworkBehaviour
{
    public VisualPathManager PathManager;


    TerrainGridSystem _tgs;
    

    public void Setup()
    {
        _tgs = TerrainGridSystem.instance;
    }

    [ObserversRpc]
    public void GeneratePath(List<VisualHex> corridors, Vector3 start, Vector3 end)
    {
        PathManager.CreateRequest(corridors, start, end);
        PathManager.GeneratePath();
        PathManager.DisplayPath();
    }

    public List<VisualHex> ConvertHexes(List<Cell> tgsCells)
    {
        List<VisualHex> listHex = new();

        foreach (Cell cell in tgsCells)
        {
            int index = cell.index;
            Vector3 center = _tgs.CellGetPosition(index);

            int count = _tgs.CellGetVertexCount(index);
            Vector3[] corners = new Vector3[count];

            for (int v = 0; v < count; v++)
            {
                Vector3 worldPos = _tgs.CellGetVertexPosition(index, v);
                corners[v] = worldPos;
            }

            listHex.Add(new VisualHex(index, center, corners));
        }

        return listHex;
    }

    
}
