using PurrNet;
using System.Collections.Generic;
using TGS;
using UnityEngine;
using VisualPath;

public class PathSystem : NetworkBehaviour
{
    public VisualPathManager PathManager;


    TerrainGridSystem _tgs;

    GameObject _currentPathBorder;

    public void Setup()
    {
        _tgs = TerrainGridSystem.instance;
    }

    [ObserversRpc]
    public void GeneratePath(List<VisualHex> corridors, Vector3 start, Vector3 end)
    {
        HighlightHexPath(corridors);

        VisualRoad road = CreateVisualRoads();
        PathManager.CreateRequest(corridors, start, end, new List<VisualRoad>() { road });
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

    private VisualRoad CreateVisualRoads()
    {
        List<int> hexIds = new();
        List<Vector3> points = new();

        foreach (Transform point in PathManager.RoadPoints)
        {
            points.Add(point.position);

            Cell cell = _tgs.CellGetAtWorldPosition(point.position);
            hexIds.Add(cell.index);
        }

        VisualRoad road = new(1, points, hexIds);
        return road;
    }

    private void HighlightHexPath(List<VisualHex> corridors)
    {
        if (_currentPathBorder != null)
            Destroy(_currentPathBorder);

        List<int> cells = new();
        foreach (VisualHex hex in corridors)
        {
            cells.Add(hex.Id);
        }

        _currentPathBorder = _tgs.CellDrawBorder(cells, Color.orange);
    }
}
