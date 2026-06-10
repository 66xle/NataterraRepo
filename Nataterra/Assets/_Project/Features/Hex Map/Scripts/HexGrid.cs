using DrawXXL;
using System.Collections.Generic;
using System.Linq;
using TGS;
using TGS.Geom;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;


public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    public TMP_Text cellLabelPrefab;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    public TerrainGridSystem tgs;

    HexCell[] cells;
    Canvas gridCanvas;

    public List<VertexData> vertices = new List<VertexData>();

    public Vector2 CellSize => tgs.cellSize / 2f;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
    }

    void Start()
    {
        tgs.SetGridType(GridTopology.Irregular);
        tgs.Redraw(true);
        tgs.highlightMode = HighlightMode.None;

        cells = new HexCell[tgs.cellCount];

        
        for (int i = 0; i < tgs.cellCount; i++)
        {
            Cell cell = tgs.cells[i];
        
            cells[i] = new HexCell(cell);
            //CreateCellLabel(cell.index, tgs.CellGetPosition(cell.index));
        }

        StoreVertices();
    }


    public void StoreVertices()
    { 
        foreach (Cell cell in tgs.cells)
        {
            int vc = tgs.CellGetVertexCount(cell.index);
            for (int i = 0; i < vc; i++)
            {
                Vector3 cellPos = tgs.CellGetVertexPosition(cell.index, i);

                VertexData existingVertex = vertices.Find(v => v.position == cellPos);
                if (existingVertex != null)
                {
                    existingVertex.AddCellIndex(cell, i);
                }
                else
                {
                    vertices.Add(new VertexData(cellPos, cell, i));
                }
            }
        }
    }


    public void SetVertexPosition(Vector3 position, VertexData vertex)
    {
        vertex.position = position;

        Vector3 localPos = tgs.transform.InverseTransformPoint(position);

        foreach ((Cell, int) cellRef in vertex.cellsRef)
        {
            tgs.cells[cellRef.Item1.index].region.points[cellRef.Item2] = new Vector2(localPos.x, localPos.y);
        }
    }

    public void RegenerateGrid(VertexData vertex)
    {
        tgs.RegenerateFlatToppedHexagonalGrid();

        foreach ((Cell, int) cellRef in vertex.cellsRef)
        {
            tgs.CellUpdateBounds(cellRef.Item1);
        }

        tgs.RedrawCells(tgs.cells);
    }

    public void RegenerateCellSurface(VertexData vertex, Color[] biomeColors)
    {
        foreach ((Cell, int) cellRef in vertex.cellsRef)
        {
            Biome biome = GetCellBiome(cellRef.Item1.index);

            Color surfaceColor = biome != Biome.None ? biomeColors[(int)biome] : Misc.ColorNull;

            tgs.CellToggleRegionSurface(cellRef.Item1.index, true, surfaceColor, true);
        }
    }

    public void ShowAllCellSurfaces()
    {
        foreach (Cell cell in tgs.cells)
        {
            tgs.CellToggleRegionSurface(cell.index, true, Misc.ColorNull, true);
        }
    }


    public bool IsCellABiome(int index)
    {
        if (cells[index].biome == Biome.None)
            return false;

        return true;
    }

    public void SetCellBiome(int index, Biome biome, Color color)
    {
        cells[index].biome = biome;

        tgs.CellToggleRegionSurface(index, true, color, true);
    }
    public void SetCellResource(int index, Resource resource, GameObject resourceObj)
    {
        if (cells[index].resourceObj != null)
        {
            Destroy(cells[index].resourceObj);
        }

        cells[index].resource = resource;
        cells[index].resourceObj = resourceObj;
    }
    public void SetCellBase(int cellIndex, int baseIndex, GameObject baseObj)
    {
        if (cells[cellIndex].baseObj != null)
        {
            Destroy(cells[cellIndex].baseObj);
        }

        HexCell cell = cells[cellIndex];

        cell.raceBase = (Base)baseIndex;
        cell.baseObj = baseObj;
    }

    public void RemoveCellBiome(int index)
    {
        cells[index].biome = Biome.None;

        tgs.CellClear(index);
    }
    public void RemoveCellResource(int cellIndex)
    {
        HexCell cell = cells[cellIndex];
        if (cell.resourceObj != null)
        {
            Destroy(cell.resourceObj);
        }

        cell.resource = Resource.None;
        cell.resourceObj = null;
    }
    public void RemoveCellBase(int cellIndex)
    {
        HexCell cell = cells[cellIndex];
        if (cell.baseObj != null)
        {
            Destroy(cell.baseObj);
        }

        cell.raceBase = Base.None;
        cell.baseObj = null;
    }

    public Biome GetCellBiome(int index)
    {
        return cells[index].biome;
    }
    public Resource GetCellResource(int index)
    {
        return cells[index].resource;
    }
    public Base GetCellBase(int index)
    {
        return cells[index].raceBase;
    }



    public int GetCellIndex(Vector3 position)
    {
        HexCoordinates coordinates = GetHexCoordinates(position);
        int x = coordinates.X;
        int z = coordinates.Z + (coordinates.X - (coordinates.X & 1)) / 2;
        int index = x + z * width;
        return index;
    }

    public HexCoordinates GetHexCoordinates(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        return HexCoordinates.FromPosition(position);
    }

    public Vector3 GetCellWorldPosition(int index)
    {
        return tgs.CellGetPosition(index);
    }

    void CreateCellLabel(int cellIndex, Vector3 position)
    {
        TMP_Text label = Instantiate<TMP_Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.position = position;

        label.rectTransform.localScale = new Vector3(label.rectTransform.localScale.x * CellSize.x, label.rectTransform.localScale.z * CellSize.y, 1f);
        label.text = cellIndex.ToString();
    }

    
}
