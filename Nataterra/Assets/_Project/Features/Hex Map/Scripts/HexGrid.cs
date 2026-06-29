using DrawXXL;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using TGS;
using TGS.Geom;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;


public class HexGrid : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] TMP_Text CellLabelPrefab;

    TerrainGridSystem TGS;
    HexCell[] _cells;
    Canvas _gridCanvas;
    List<VertexData> _vertices = new List<VertexData>();

    public Canvas GridCanvas { get { return _gridCanvas; } }
    public List<VertexData> Vertices { get { return _vertices; } }
    public Vector2 CellSize => TGS.cellSize / 2f;


    void Awake()
    {
        _gridCanvas = GetComponentInChildren<Canvas>();
    }

    void Start()
    {
        TGS = TerrainGridSystem.instance;
        TGS.SetGridType(GridTopology.Irregular);
        TGS.Redraw(true);
        TGS.highlightMode = HighlightMode.None;

        _cells = new HexCell[TGS.cellCount];

        for (int i = 0; i < TGS.cellCount; i++)
        {
            _cells[i] = new HexCell();
            //CreateCellLabel(cell.index, tgs.CellGetPosition(cell.index));
        }

        StoreVertices();
    }

    public void SetCells(HexCell[] cells)
    {
        this._cells = cells;
    }

    public void StoreVertices()
    {
        _vertices.Clear();

        foreach (Cell cell in TGS.cells)
        {
            int vc = TGS.CellGetVertexCount(cell.index);
            for (int i = 0; i < vc; i++)
            {
                Vector3 cellPos = TGS.CellGetVertexPosition(cell.index, i);

                VertexData existingVertex = _vertices.Find(v => v.position == cellPos);
                if (existingVertex != null)
                {
                    existingVertex.AddCellIndex(cell, i);
                }
                else
                {
                    _vertices.Add(new VertexData(cellPos, cell, i));
                }
            }
        }
    }


    public void SetVertexPosition(Vector3 position, VertexData vertex)
    {
        vertex.position = position;

        Vector3 localPos = TGS.transform.InverseTransformPoint(position);

        foreach ((Cell, int) cellRef in vertex.cellsRef)
        {
            TGS.cells[cellRef.Item1.index].region.points[cellRef.Item2] = new Vector2(localPos.x, localPos.y);
        }
    }

    public void RegenerateGrid(VertexData vertex)
    {
        TGS.RegenerateFlatToppedHexagonalGrid();

        foreach ((Cell, int) cellRef in vertex.cellsRef)
        {
            TGS.CellUpdateBounds(cellRef.Item1);
        }

        TGS.RedrawCells(TGS.cells);
    }
    public void RegenerateGrid(List<CellData> tgsCells)
    {
        TGS.RegenerateFlatToppedHexagonalGrid(tgsCells);
        TGS.CellsUpdateBounds();
        TGS.CellsUpdateNeighbours();
        TGS.RedrawCells(TGS.cells);

        StoreVertices();
    }

    public void RegenerateCellSurface(VertexData vertex, Texture2D[] biomeTextures, Texture2D[] baseTextures)
    {
        foreach ((Cell, int) cellRef in vertex.cellsRef)
        {
            int cellIndex = cellRef.Item1.index;

            Texture2D surfaceTexture;

            // Set either biome or base
            if (IsCellABase(cellIndex))
            {
                Base faction = GetCellBase(cellIndex);
                surfaceTexture = faction != Base.None ? baseTextures[(int)faction] : null;
            }
            else
            {
                Biome biome = GetCellBiome(cellIndex);
                surfaceTexture = biome != Biome.None ? biomeTextures[(int)biome] : null;
            }

            if (surfaceTexture == null)
            {
                // Empty cell
                TGS.CellToggleRegionSurface(cellIndex, true, Misc.ColorNull, true);
                continue;
            }

            // Biome or base cell
            TGS.CellToggleRegionSurface(cellIndex, true, Color.white, true, surfaceTexture);

            // Set resource position if it exists
            if (DoesResourceExist(cellIndex))
                _cells[cellIndex].resourceObj.transform.position = GetCellWorldPosition(cellIndex);
        }
    }

    public void ShowAllCellSurfaces()
    {
        foreach (Cell cell in TGS.cells)
        {
            TGS.CellToggleRegionSurface(cell.index, true, Misc.ColorNull, true);
        }
    }


    public bool IsCellABiome(int index)
    {
        if (_cells[index].biome == Biome.None)
            return false;

        return true;
    }
    public bool DoesResourceExist(int index)
    {
        if (_cells[index].resourceObj == null)
            return false;

        return true;
    }

    public bool IsCellABase(int index)
    {
        if (_cells[index].faction == Base.None)
            return false;

        return true;
    }


    public void SetCellBiome(int index, Biome biome, Texture2D texture = null)
    {
        _cells[index].biome = biome;

        if (texture != null)
            TGS.CellToggleRegionSurface(index, true, Color.white, true, texture);
    }
    public void SetCellResource(int index, Resource resource, GameObject resourceObj)
    {
        if (_cells[index].resourceObj != null)
        {
            Destroy(_cells[index].resourceObj);
        }

        _cells[index].resource = resource;
        _cells[index].resourceObj = resourceObj;
    }
    public void SetCellBase(int cellIndex, Base faction, Texture2D texture)
    {
        _cells[cellIndex].faction = faction;

        TGS.CellToggleRegionSurface(cellIndex, true, Color.white, true, texture);
    }

    public void RemoveCellBiome(int index)
    {
        _cells[index].biome = Biome.None;

        TGS.CellClear(index);
    }
    public void RemoveCellResource(int cellIndex)
    {
        HexCell cell = _cells[cellIndex];
        if (cell.resourceObj != null)
        {
            Destroy(cell.resourceObj);
        }

        cell.resource = Resource.None;
        cell.resourceObj = null;
    }
    public void RemoveCellBase(int cellIndex)
    {
        _cells[cellIndex].faction = Base.None;

        TGS.CellClear(cellIndex);
    }

    public Biome GetCellBiome(int index)
    {
        return _cells[index].biome;
    }
    public Resource GetCellResource(int index)
    {
        return _cells[index].resource;
    }
    public Base GetCellBase(int index)
    {
        return _cells[index].faction;
    }


    public HexCell[] GetHexCells()
    {
        return _cells;
    }

    public int GetCellIndex(Vector3 position)
    {
        HexCoordinates coordinates = GetHexCoordinates(position);
        int x = coordinates.X;
        int z = coordinates.Z + (coordinates.X - (coordinates.X & 1)) / 2;
        int index = x + z /* * width */;
        return index;
    }

    public HexCoordinates GetHexCoordinates(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        return HexCoordinates.FromPosition(position);
    }

    public Vector3 GetCellWorldPosition(int index)
    {
        return TGS.CellGetCentroid(index);
    }

    void CreateCellLabel(int cellIndex, Vector3 position)
    {
        TMP_Text label = Instantiate<TMP_Text>(CellLabelPrefab);
        label.rectTransform.SetParent(_gridCanvas.transform, false);
        label.rectTransform.position = position;

        label.rectTransform.localScale = new Vector3(label.rectTransform.localScale.x * CellSize.x, label.rectTransform.localScale.z * CellSize.y, 1f);
        label.text = cellIndex.ToString();
    }

    
}
