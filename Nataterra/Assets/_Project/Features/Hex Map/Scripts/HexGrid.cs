using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TGS;
using UnityEditor.Rendering;

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
    HexMesh hexMesh;

    public Vector2 CellSize => tgs.cellSize / 2f;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
    }

    void Start()
    {
        tgs.Redraw();
        tgs.highlightMode = HighlightMode.None;

        cells = new HexCell[tgs.cellCount];

        for (int i = 0; i < tgs.cellCount; i++)
        {
            Cell cell = tgs.cells[i];

            cells[i] = new HexCell(cell);
            CreateCellLabel(cell.index, tgs.CellGetPosition(cell.index));
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

        tgs.CellSetColor(index, color);
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
