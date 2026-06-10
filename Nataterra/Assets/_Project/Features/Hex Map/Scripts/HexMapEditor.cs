using DrawXXL;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using TGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Tab
{
    Select = 0,
    Biome = 1,
    Resource = 2,
    Base = 3,
    Vertex = 4,
}


public class HexMapEditor : MonoBehaviour
{
    public Color[] biomeColors;
    public Color[] resourceColors;
    public Texture2D[] resourceTextures;

    public List<Button> buttons;
    public List<GameObject> panels;

    public TMP_Text selectedTileText;
    public Toggle disableToggle;

    public GameObject prefab;
    public Vector3 resourceScale = Vector3.one;
    public Vector3 baseScale = new Vector3(1.5f, 1.5f, 1.5f);

    public float vertexDetectionRadius = 1f;

    public LayerMask hexGridLayer;

    public HexGrid hexGrid;



    int[] basesPlaced = new int[4] { -1, -1, -1, -1 };
    ToggleGroup[] selectToggleGroups = new ToggleGroup[3];

    Tab currentTab;
    GameObject selectedCell;

    int currentCellIndex;
    VertexData selectedVertex;

    bool isDraggingVertex;
    Vector3 currentVertexPos;
    Vector3 oldVertexPos;

    int biomeIndex;
    int resourceIndex;
    int baseIndex;

    void Awake()
    {
        SelectButton(0);
        biomeIndex = 0;
        resourceIndex = 0;
        baseIndex = 0;

        selectToggleGroups = panels[0].GetComponentsInChildren<ToggleGroup>();
    }

    private void Update()
    {
        HandleInput();

        if (currentTab == Tab.Vertex)
        {
            if (!isDraggingVertex)
                CheckMouseNearVertex();

            if (isDraggingVertex)
            {
                currentVertexPos = MoveVertex(currentVertexPos);

                bool isWithinBounds = false;

                Cell cell = hexGrid.tgs.CellGetAtMousePosition();
                if (cell != null)
                {
                    isWithinBounds = selectedVertex.cellsRef.Any(c => c.Item1.index == cell.index);
                    Color distanceColor = isWithinBounds ? Color.green : Color.red;
                    DrawMeasurements.Distance(oldVertexPos, currentVertexPos, distanceColor);
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    isDraggingVertex = false;

                    if (!isWithinBounds) return;
                    hexGrid.SetVertexPosition(currentVertexPos, selectedVertex);
                    hexGrid.RegenerateGrid(selectedVertex);
                    hexGrid.RegenerateCellSurface(selectedVertex, biomeColors);
                }
            }
        }
    }



    void HandleInput()
    {
        Cell cell = hexGrid.tgs.CellGetAtMousePosition();

        if (cell == null) return;

        int cellIndex = cell.index;


        if (Input.GetKey(KeyCode.Mouse0) && currentTab != Tab.Select || Input.GetKeyDown(KeyCode.Mouse0) && currentTab == Tab.Select)
        {
            if (Tab.Select == currentTab)
            {
                ShowTileDetails(cellIndex);
            }
            else if (Tab.Biome == currentTab)
            {
                ChangeTileBiome(cellIndex);
            }
            else if (Tab.Resource == currentTab)
            {
                ChangeTileResource(cellIndex);
            }
            else if (Tab.Base == currentTab)
            {
                ChangeTileBase(cellIndex);
            }
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            hexGrid.RemoveCellBiome(cellIndex);
            hexGrid.RemoveCellResource(cellIndex);
            hexGrid.RemoveCellBase(cellIndex);

            if (currentTab == Tab.Select)
            {
                SetCellToggles(cellIndex);

                // When resetting current cell which is no longer a biome, so hide resource toggles
                if (currentCellIndex == cellIndex)
                    selectToggleGroups[1].gameObject.SetActive(false);
            }
        }
    }

    public void SelectButton(int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == index)
            {
                buttons[i].interactable = false;
                Tab previousTab = currentTab;
                currentTab = (Tab)index;

                // If clicked on tabs other than select
                if (currentTab != Tab.Select)
                {
                    if (i < panels.Count)
                        panels[i].SetActive(true);

                    if (selectedCell != null)
                        Destroy(selectedCell);
                }
            }
            else
            {
                buttons[i].interactable = true;

                if (i < panels.Count)
                    panels[i].SetActive(false);
            }
        }
    }

    private void ShowTileDetails(int cellIndex)
    {
        if (selectedCell != null)
            Destroy(selectedCell);

        selectedCell = hexGrid.tgs.CellDrawBorder(cellIndex, Color.blue, 1f);
        selectedTileText.text = $"Selected: {cellIndex}";

        currentCellIndex = cellIndex;
        panels[0].SetActive(true); // Select panel

        // Show resource toggles if cell is a biome, otherwise hide it
        bool isBiome = hexGrid.IsCellABiome(cellIndex);
        selectToggleGroups[1].gameObject.SetActive(isBiome);

        SetCellToggles(cellIndex);
    }

    private void SetCellToggles(int cellIndex)
    {
        Biome biome = hexGrid.GetCellBiome(cellIndex);
        Resource resource = hexGrid.GetCellResource(cellIndex);
        Base raceBase = hexGrid.GetCellBase(cellIndex);

        selectToggleGroups[0].SetAllTogglesOff(false);
        selectToggleGroups[1].SetAllTogglesOff(false);
        selectToggleGroups[2].SetAllTogglesOff(false);

        if (Biome.None != biome)
            selectToggleGroups[0].GetComponentsInChildren<Toggle>()[(int)biome].SetIsOnWithoutNotify(true);

        if (Resource.None != resource)
            selectToggleGroups[1].GetComponentsInChildren<Toggle>()[(int)resource].SetIsOnWithoutNotify(true);

        if (Base.None != raceBase)
            selectToggleGroups[2].GetComponentsInChildren<Toggle>()[(int)raceBase].SetIsOnWithoutNotify(true);
    }



    private void CheckMouseNearVertex()
    {
        Cell cell = hexGrid.tgs.CellGetAtMousePosition();

        if (cell == null) return;

        int vc = hexGrid.tgs.CellGetVertexCount(cell.index);

        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, hexGridLayer))
        {
            for (int i = 0; i < vc; i++)
            {
                Vector3 vertPos = hexGrid.tgs.CellGetVertexPosition(cell.index, i);

                Debug.DrawRay(vertPos, Vector3.up * 5f, Color.yellow);

                Vector3 checkPos = hit.point;
                checkPos.y = vertPos.y;

                if (Vector3.Distance(checkPos, vertPos) < vertexDetectionRadius && Vector3.Distance(checkPos, vertPos) > -vertexDetectionRadius)
                {
                    // Draw axis on vertex pos
                    DrawEngineBasics.CoordinateAxesGizmo(vertPos, 10f);

                    VertexData vertex = hexGrid.vertices.FirstOrDefault(v => (v.position - vertPos).sqrMagnitude < 0.1f);
                    foreach ((Cell, int) cellRef in vertex.cellsRef)
                    {
                        Vector3 pos = hexGrid.tgs.CellGetCentroid(cellRef.Item1.index);

                        // Draw cell position sharing vertex
                        DrawBasics.Vector(pos, pos + Vector3.up * 5f);
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        oldVertexPos = checkPos;
                        currentVertexPos = checkPos;

                        selectedVertex = vertex;
                        isDraggingVertex = true;
                    }

                    
                }
            }
        }
    }

    private Vector3 MoveVertex(Vector3 position)
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, hexGridLayer))
        {
            return hit.point;
        }

        Camera cam = Camera.main;

        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.Normalize();
        right.Normalize();

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float speed = 5f;

        position += right * mouseX * speed * Time.deltaTime;
        position += forward * mouseY * speed * Time.deltaTime;

        return position;
    }


    public void ChangeCellType(int typeIndex)
    { 
        if (typeIndex == 0)
        {
            ChangeTileBiome(currentCellIndex);
        }
        else if (typeIndex == 1)
        {
            ChangeTileResource(currentCellIndex);
        }
        else if (typeIndex == 2)
        {
            ChangeTileBase(currentCellIndex);
        }
        else
        {
            Debug.LogError("Incorrect TypeIndex: " + typeIndex);
        }
    }

    private void ChangeTileBiome(int cellIndex)
    {
        hexGrid.SetCellBiome(cellIndex, (Biome)biomeIndex, biomeColors[biomeIndex]);

        if (currentTab == Tab.Select)
        {
            SetCellToggles(cellIndex);

            // Show resource toggles
            selectToggleGroups[1].gameObject.SetActive(true);
        }
    }

    private void ChangeTileResource(int cellIndex)
    {
        if (!hexGrid.IsCellABiome(cellIndex)) return;

        Vector3 cellPosition = hexGrid.GetCellWorldPosition(cellIndex);

        GameObject resourceObj = Instantiate(prefab, cellPosition, Quaternion.identity);
        resourceObj.transform.localScale = new Vector3(resourceScale.x * hexGrid.CellSize.x, resourceScale.y, resourceScale.z * hexGrid.CellSize.y);
        resourceObj.GetComponent<Renderer>().material.color = resourceColors[resourceIndex];

        hexGrid.RemoveCellBase(cellIndex);
        hexGrid.SetCellResource(cellIndex, (Resource)resourceIndex, resourceObj);

        if (currentTab == Tab.Select)
            SetCellToggles(cellIndex);
    }

    private void ChangeTileBase(int cellIndex)
    {
        Vector3 cellPosition = hexGrid.GetCellWorldPosition(cellIndex);

        GameObject baseObj = Instantiate(prefab, cellPosition, Quaternion.identity);
        baseObj.transform.localScale = new Vector3(baseScale.x * hexGrid.CellSize.x, baseScale.y, baseScale.z * hexGrid.CellSize.y);
        baseObj.GetComponent<Renderer>().material.color = Color.white;

        for (int i = 0; i < basesPlaced.Length; i++)
        {
            if (basesPlaced[i] != cellIndex) continue;

            basesPlaced[i] = -1;
        }

        // Only one base should exist per type
        if (basesPlaced[baseIndex] != -1)
            hexGrid.RemoveCellBase(basesPlaced[baseIndex]);

        hexGrid.RemoveCellResource(cellIndex);
        hexGrid.SetCellBiome(cellIndex, (Biome)baseIndex, biomeColors[baseIndex]);
        hexGrid.SetCellBase(cellIndex, baseIndex, baseObj);

        basesPlaced[baseIndex] = cellIndex;

        if (currentTab == Tab.Select)
            SetCellToggles(cellIndex);
    }

    


    public void SetBiome(int index)
    {
        biomeIndex = index;
    }
    public void SetResource(int index)
    {
        resourceIndex = index;
    }
    public void SetBase(int index)
    {
        baseIndex = index;
    }
}
