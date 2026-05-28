using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Vector2Int GridSize = new(10, 10);
    public Vector2 TileSize = new(1, 1);


    Dictionary<Vector2Int, TileData> _tileDictionary = new();

    // Editor
    private Vector2Int _hoveredTile;
    private bool _hasHover;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PopulateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateGrid()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                _tileDictionary.Add(new Vector2Int(x, y), GenerateTile());
            }
        }
    }

    TileData GenerateTile()
    {
        return new TileData();
    }

    private void OnDrawGizmos()
    {
        if (_tileDictionary == null || _tileDictionary.Count == 0)
            return;

        UpdateHoverTile();

        foreach (var tile in _tileDictionary)
        {
            Vector2Int coord = tile.Key;

            bool isHovered = _hasHover && coord == _hoveredTile;

            DrawHex(coord, isHovered);
            DrawCoordinateLabel(coord);
        }
    }

    private void UpdateHoverTile()
    {
        if (Camera.current == null)
            return;

        Event e = Event.current;
        if (e == null)
            return;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        Plane ground = new Plane(Vector3.up, Vector3.zero);

        if (ground.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);

            Vector2Int coord = WorldToOffset(hit);

            _hasHover = _tileDictionary.ContainsKey(coord);

            if (_hasHover)
                _hoveredTile = coord;
        }
    }

    private void DrawCoordinateLabel(Vector2Int coord)
    {
        Vector3 center = OffsetToWorld(coord);

        GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
        style.alignment = TextAnchor.MiddleCenter;
        style.richText = true;

        string label = $"(<color=red>{coord.x}</color>," + $"<color=cyan>{coord.y}</color>)";
        Handles.Label(center + Vector3.up * 0.1f, label, style);
    }

    private void DrawHex(Vector2Int axial, bool hovered)
    {
        Vector3 center = OffsetToWorld(axial);

        Vector3[] corners = new Vector3[6];

        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60f * i);

            corners[i] = center + new Vector3(TileSize.x * Mathf.Cos(angle), 0f, TileSize.y * Mathf.Sin(angle));
        }

        Gizmos.color = hovered ? Color.yellow : Color.green;

        for (int i = 0; i < 6; i++)
        {
            Gizmos.DrawLine(corners[i], corners[(i + 1) % 6]);
        }

        Gizmos.color = Color.green;
    }

    private Vector3 OffsetToWorld(Vector2Int coord)
    {
        int col = coord.x;
        int row = coord.y;

        float x = col * (TileSize.x * 1.5f);
        float z = row * (TileSize.y * Mathf.Sqrt(3f));

        // Even columns shifted down
        if ((col & 1) == 0)
        {
            z += TileSize.y * Mathf.Sqrt(3f) * 0.5f;
        }

        // Top of map = negative Z
        z = -z;

        return new Vector3(x, 0f, z);
    }

    private Vector2Int WorldToOffset(Vector3 worldPos)
    {
        float colF = worldPos.x / (TileSize.x * 1.5f);

        float rowF =
            (-worldPos.z / (TileSize.y * Mathf.Sqrt(3f))) -
            (colF * 0.5f);

        // cube coords (even-q -> cube)
        float x = colF;
        float z = rowF;
        float y = -x - z;

        int rx = Mathf.RoundToInt(x);
        int ry = Mathf.RoundToInt(y);
        int rz = Mathf.RoundToInt(z);

        float dx = Mathf.Abs(rx - x);
        float dy = Mathf.Abs(ry - y);
        float dz = Mathf.Abs(rz - z);

        // cube rounding correction
        if (dx > dy && dx > dz)
            rx = -ry - rz;
        else if (dy > dz)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        // cube -> even-q offset
        int col = rx;
        int row = rz + (rx - (rx & 1)) / 2;

        return new Vector2Int(col, row);
    }

}
