using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    public TMP_Text cellLabelPrefab;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    HexCell[] cells;
    Canvas gridCanvas;
    HexMesh hexMesh;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
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
        cells[index].color = color;
        hexMesh.Triangulate(cells);
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
        HexCell cell = cells[index];
        cell.biome = Biome.None;
        cell.color = Color.white;
        hexMesh.Triangulate(cells);
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

    public Vector3 GetCellPosition(int index)
    {
        return HexCoordinates.ToPosition(cells[index].coordinates);
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.y = 0f;
        position.z = (z + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        TMP_Text label = Instantiate<TMP_Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();

    }

    
}
