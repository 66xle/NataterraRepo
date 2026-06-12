using DrawXXL;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using TGS;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
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
    public Texture2D[] biomeTextures;
    public Texture2D[] resourceTextures;
    public Texture2D[] baseTextures;

    public List<Button> buttons;
    public List<GameObject> panels;

    public TMP_Text selectedTileText;

    public GameObject prefab;
    public GameObject resourceImagePrefab;

    public float vertexDetectionRadius = 1f;
    public LayerMask hexGridLayer;
    public HexGrid hexGrid;

    // Options
    public TMP_InputField inputField;
    public TMP_Text errorText;


    // Private
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

    HexMapOptions options;

    void Awake()
    {
        options = new(this);

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
                    hexGrid.RegenerateCellSurface(selectedVertex, biomeTextures, baseTextures);
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
                SetSelectToggles(cellIndex);

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

                if (currentTab == Tab.Biome || currentTab == Tab.Resource || currentTab == Tab.Base)
                {
                    Toggle[] toggles = panels[index].GetComponentsInChildren<Toggle>();

                    for (int t = 0; t < toggles.Length; t++)
                    {
                        if (!toggles[t].isOn)
                            continue;

                        if (Tab.Biome == currentTab)
                            biomeIndex = t;
                        else if (Tab.Resource == currentTab)
                            resourceIndex = t;
                        else if (Tab.Base == currentTab)
                            baseIndex = t;
                    }
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

        SetSelectToggles(cellIndex);
    }

    private void SetSelectToggles(int cellIndex)
    {
        Biome biome = hexGrid.GetCellBiome(cellIndex);
        Resource resource = hexGrid.GetCellResource(cellIndex);
        Base faction = hexGrid.GetCellBase(cellIndex);

        selectToggleGroups[0].SetAllTogglesOff(false);
        selectToggleGroups[1].SetAllTogglesOff(false);
        selectToggleGroups[2].SetAllTogglesOff(false);

        if (Biome.None != biome)
            selectToggleGroups[0].GetComponentsInChildren<Toggle>()[(int)biome].SetIsOnWithoutNotify(true);

        if (Resource.None != resource)
            selectToggleGroups[1].GetComponentsInChildren<Toggle>()[(int)resource].SetIsOnWithoutNotify(true);

        if (Base.None != faction)
            selectToggleGroups[2].GetComponentsInChildren<Toggle>()[(int)faction].SetIsOnWithoutNotify(true);
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
        // Remove base
        hexGrid.RemoveCellBase(cellIndex);
        selectToggleGroups[2].SetAllTogglesOff(false);

        // Set biome
        hexGrid.SetCellBiome(cellIndex, (Biome)biomeIndex, biomeTextures[biomeIndex]);

        if (currentTab == Tab.Select || currentTab == Tab.Biome)
        {
            SetSelectToggles(cellIndex);

            bool enableResource = true;

            if (Biome.Mountain == (Biome)biomeIndex || Biome.Lake == (Biome)biomeIndex)
            {
                enableResource = false;
                hexGrid.RemoveCellResource(cellIndex);
            }
            
            if (currentTab == Tab.Select)
                selectToggleGroups[1].gameObject.SetActive(enableResource);

            
        }
    }

    private void ChangeTileResource(int cellIndex)
    {
        if (!hexGrid.IsCellABiome(cellIndex)) return;

        Biome biome = hexGrid.GetCellBiome(cellIndex);
        if (biome == Biome.Mountain || biome == Biome.Lake)
            return;

        Vector3 cellPosition = hexGrid.GetCellWorldPosition(cellIndex);

        GameObject resourceObj = Instantiate(resourceImagePrefab);
        resourceObj.transform.position = cellPosition;

        RawImage resourceImage = resourceObj.GetComponent<RawImage>();
        resourceImage.rectTransform.SetParent(hexGrid.gridCanvas.transform, false);
        resourceImage.rectTransform.localScale = new Vector3(resourceImage.rectTransform.localScale.x * hexGrid.CellSize.x, resourceImage.rectTransform.localScale.z * hexGrid.CellSize.y, 1f);
        resourceImage.texture = resourceTextures[resourceIndex];

        hexGrid.RemoveCellBase(cellIndex);
        hexGrid.SetCellBiome(cellIndex, biome, biomeTextures[(int)biome]);
        hexGrid.SetCellResource(cellIndex, (Resource)resourceIndex, resourceObj);

        if (currentTab == Tab.Select)
            SetSelectToggles(cellIndex);
    }

    private void ChangeTileBase(int cellIndex)
    {
        Vector3 cellPosition = hexGrid.GetCellWorldPosition(cellIndex);

        for (int i = 0; i < basesPlaced.Length; i++)
        {
            // Skip if base is placed
            if (basesPlaced[i] != cellIndex) continue;

            // If a base is not placed set to -1
            basesPlaced[i] = -1;
        }

        // Only one base should exist per type
        if (basesPlaced[baseIndex] != -1)
            hexGrid.RemoveCellBase(basesPlaced[baseIndex]);

        hexGrid.RemoveCellResource(cellIndex);
        hexGrid.SetCellBiome(cellIndex, (Biome)baseIndex);
        hexGrid.SetCellBase(cellIndex, (Base)baseIndex, baseTextures[baseIndex]);

        basesPlaced[baseIndex] = cellIndex;

        if (currentTab == Tab.Select)
            SetSelectToggles(cellIndex);
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


    public void SaveMap()
    {
        options.SaveMap(inputField.text, hexGrid.GetHexCells(), hexGrid.tgs.cells);
    }
}
