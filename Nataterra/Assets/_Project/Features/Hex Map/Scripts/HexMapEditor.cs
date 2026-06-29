using DrawXXL;
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
    [Header("Textures")]
    public Texture2D[] BiomeTextures;
    public Texture2D[] ResourceTextures;
    public Texture2D[] BaseTextures;

    [Header("Toolbar")]
    public List<Button> Buttons;
    public List<GameObject> Panels;

    [Header("References")]
    public TMP_Text SelectedTileText;
    public GameObject ResourceImagePrefab;

    [Header("Interact Options")]
    public float VertexDetectionRadius = 1f;
    public LayerMask HexGridLayer;
    public HexGrid HexGrid;

    [Header("Save and Load UI")]
    public TMP_InputField InputField;
    public TMP_Text ErrorText;
    public TMP_Dropdown Dropdown;


    // Private
    int[] _basesPlaced = new int[4] { -1, -1, -1, -1 };
    ToggleGroup[] _selectToggleGroups = new ToggleGroup[3];

    Tab _currentTab;
    GameObject _selectedCell;

    int _currentCellIndex;
    VertexData _selectedVertex;

    bool _isDraggingVertex;
    Vector3 _currentVertexPos;
    Vector3 _oldVertexPos;

    int _biomeIndex;
    int _resourceIndex;
    int _baseIndex;

    HexMapOptions _options;

    TerrainGridSystem TGS;

    void Awake()
    {
        TGS = TerrainGridSystem.instance;

        _options = new(this);

        SelectButton(0);
        _biomeIndex = 0;
        _resourceIndex = 0;
        _baseIndex = 0;

        // Get Select panel toggle groups
        _selectToggleGroups = Panels[0].GetComponentsInChildren<ToggleGroup>();

        Dropdown.AddOptions(Extensions.SceneGetList());
    }

    private void Update()
    {
        HandleInput();

        if (_currentTab == Tab.Vertex)
        {
            if (!_isDraggingVertex)
                CheckMouseNearVertex();

            if (_isDraggingVertex)
            {
                _currentVertexPos = MoveVertex(_currentVertexPos);

                bool isWithinBounds = false;

                
                Cell cell = TGS.CellGetAtMousePosition();
                if (cell != null)
                {
                    // Check if vertex is in bounds
                    isWithinBounds = _selectedVertex.cellsRef.Any(c => c.Item1.index == cell.index);

                    // Debug distance from last position and is within bounds
                    Color distanceColor = isWithinBounds ? Color.green : Color.red;
                    DrawMeasurements.Distance(_oldVertexPos, _currentVertexPos, distanceColor);
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    _isDraggingVertex = false;

                    if (!isWithinBounds) return;

                    // Set vertex position and update grid
                    HexGrid.SetVertexPosition(_currentVertexPos, _selectedVertex);
                    HexGrid.RegenerateGrid(_selectedVertex);
                    HexGrid.RegenerateCellSurface(_selectedVertex, BiomeTextures, BaseTextures);
                }
            }
        }
    }

    


    void HandleInput()
    {
        Cell cell = TGS.CellGetAtMousePosition();

        if (cell == null) return;

        int cellIndex = cell.index;


        if (Input.GetKey(KeyCode.Mouse0) && _currentTab != Tab.Select || Input.GetKeyDown(KeyCode.Mouse0) && _currentTab == Tab.Select)
        {
            if (Tab.Select == _currentTab)
            {
                ShowTileDetails(cellIndex);
            }
            else if (Tab.Biome == _currentTab)
            {
                ChangeTileBiome(cellIndex);
            }
            else if (Tab.Resource == _currentTab)
            {
                ChangeTileResource(cellIndex);
            }
            else if (Tab.Base == _currentTab)
            {
                ChangeTileBase(cellIndex);
            }
        }
        else if (Input.GetKey(KeyCode.Mouse1) && Input.GetKey(KeyCode.LeftControl))
        {
            ResetCell(cellIndex);
        }
    }

    public void ResetCell(int cellIndex)
    {
        HexGrid.RemoveCellBiome(cellIndex);
        HexGrid.RemoveCellResource(cellIndex);
        HexGrid.RemoveCellBase(cellIndex);

        if (_currentTab == Tab.Select)
        {
            SetSelectToggles(cellIndex);

            // When resetting current cell which is no longer a biome, so hide resource toggles
            if (_currentCellIndex == cellIndex)
                _selectToggleGroups[1].gameObject.SetActive(false);
        }
    }

    public void SelectButton(int index)
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i == index)
            {
                Buttons[i].interactable = false;
                Tab previousTab = _currentTab;
                _currentTab = (Tab)index;

                // If clicked on tabs other than select
                if (_currentTab != Tab.Select)
                {
                    if (i < Panels.Count)
                        Panels[i].SetActive(true);

                    if (_selectedCell != null)
                        Destroy(_selectedCell);
                }

                if (_currentTab == Tab.Biome || _currentTab == Tab.Resource || _currentTab == Tab.Base)
                {
                    Toggle[] toggles = Panels[index].GetComponentsInChildren<Toggle>();

                    // Match tabs toggles with respective index
                    for (int t = 0; t < toggles.Length; t++)
                    {
                        if (!toggles[t].isOn)
                            continue;

                        if (Tab.Biome == _currentTab)
                            _biomeIndex = t;
                        else if (Tab.Resource == _currentTab)
                            _resourceIndex = t;
                        else if (Tab.Base == _currentTab)
                            _baseIndex = t;
                    }
                }
            }
            else
            {
                Buttons[i].interactable = true;

                if (i < Panels.Count)
                    Panels[i].SetActive(false);
            }
        }
    }

    private void ShowTileDetails(int cellIndex)
    {
        if (_selectedCell != null)
            Destroy(_selectedCell);

        _selectedCell = TGS.CellDrawBorder(cellIndex, Color.blue, 1f);
        SelectedTileText.text = $"Selected: {cellIndex}";

        _currentCellIndex = cellIndex;
        Panels[0].SetActive(true); // Select panel

        // Show resource toggles if cell is a biome, otherwise hide it
        bool isBiome = HexGrid.IsCellABiome(cellIndex);
        _selectToggleGroups[1].gameObject.SetActive(isBiome);

        SetSelectToggles(cellIndex);
    }

    private void SetSelectToggles(int cellIndex)
    {
        Biome biome = HexGrid.GetCellBiome(cellIndex);
        Resource resource = HexGrid.GetCellResource(cellIndex);
        Base faction = HexGrid.GetCellBase(cellIndex);

        _selectToggleGroups[0].SetAllTogglesOff(false);
        _selectToggleGroups[1].SetAllTogglesOff(false);
        _selectToggleGroups[2].SetAllTogglesOff(false);

        if (Biome.None != biome)
            _selectToggleGroups[0].GetComponentsInChildren<Toggle>()[(int)biome].SetIsOnWithoutNotify(true);

        if (Resource.None != resource)
            _selectToggleGroups[1].GetComponentsInChildren<Toggle>()[(int)resource].SetIsOnWithoutNotify(true);

        if (Base.None != faction)
            _selectToggleGroups[2].GetComponentsInChildren<Toggle>()[(int)faction].SetIsOnWithoutNotify(true);
    }



    private void CheckMouseNearVertex()
    {
        Cell cell = TGS.CellGetAtMousePosition();

        if (cell == null) return;

        int vc = TGS.CellGetVertexCount(cell.index);

        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, HexGridLayer))
        {
            // Loop through the cell's vertices
            for (int i = 0; i < vc; i++)
            {
                Vector3 vertPos = TGS.CellGetVertexPosition(cell.index, i);

                Debug.DrawRay(vertPos, Vector3.up * 5f, Color.yellow);

                Vector3 checkPos = hit.point;
                checkPos.y = vertPos.y;

                if (Vector3.Distance(checkPos, vertPos) < VertexDetectionRadius && Vector3.Distance(checkPos, vertPos) > -VertexDetectionRadius)
                {
                    // Debug draw axis on vertex position
                    DrawEngineBasics.CoordinateAxesGizmo(vertPos, 10f);

                    // Get matching vertex in hexGrid
                    VertexData vertex = HexGrid.Vertices.FirstOrDefault(v => (v.position - vertPos).sqrMagnitude < 0.1f);
                    foreach ((Cell, int) cellRef in vertex.cellsRef)
                    {
                        Vector3 pos = TGS.CellGetCentroid(cellRef.Item1.index);

                        // Draw cell position sharing vertex
                        DrawBasics.Vector(pos, pos + Vector3.up * 5f);
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        _oldVertexPos = checkPos;
                        _currentVertexPos = checkPos;

                        _selectedVertex = vertex;
                        _isDraggingVertex = true;
                    }

                    
                }
            }
        }
    }

    private Vector3 MoveVertex(Vector3 position)
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, HexGridLayer))
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
            ChangeTileBiome(_currentCellIndex);
        }
        else if (typeIndex == 1)
        {
            ChangeTileResource(_currentCellIndex);
        }
        else if (typeIndex == 2)
        {
            ChangeTileBase(_currentCellIndex);
        }
        else
        {
            Debug.LogError("Incorrect TypeIndex: " + typeIndex);
        }
    }

    private void ChangeTileBiome(int cellIndex)
    {
        // Remove base
        HexGrid.RemoveCellBase(cellIndex);
        _selectToggleGroups[2].SetAllTogglesOff(false);

        // Set biome
        HexGrid.SetCellBiome(cellIndex, (Biome)_biomeIndex, BiomeTextures[_biomeIndex]);

        if (_currentTab == Tab.Select || _currentTab == Tab.Biome)
        {
            bool enableResource = true;

            if (Biome.Mountain == (Biome)_biomeIndex || Biome.Lake == (Biome)_biomeIndex)
            {
                enableResource = false;
                HexGrid.RemoveCellResource(cellIndex);
            }
            
            if (_currentTab == Tab.Select)
            {
                SetSelectToggles(cellIndex);
                _selectToggleGroups[1].gameObject.SetActive(enableResource);
            }
        }
    }

    private void ChangeTileResource(int cellIndex)
    {
        if (!HexGrid.IsCellABiome(cellIndex)) return;

        Biome biome = HexGrid.GetCellBiome(cellIndex);
        if (biome == Biome.Mountain || biome == Biome.Lake)
            return;

        Vector3 cellPosition = HexGrid.GetCellWorldPosition(cellIndex);

        GameObject resourceObj = Instantiate(ResourceImagePrefab);
        resourceObj.transform.position = cellPosition;

        RawImage resourceImage = resourceObj.GetComponent<RawImage>();
        resourceImage.rectTransform.SetParent(HexGrid.GridCanvas.transform, false);
        resourceImage.rectTransform.localScale = new Vector3(resourceImage.rectTransform.localScale.x * HexGrid.CellSize.x, resourceImage.rectTransform.localScale.z * HexGrid.CellSize.y, 1f);
        resourceImage.texture = ResourceTextures[_resourceIndex];

        HexGrid.RemoveCellBase(cellIndex);
        HexGrid.SetCellBiome(cellIndex, biome, BiomeTextures[(int)biome]);
        HexGrid.SetCellResource(cellIndex, (Resource)_resourceIndex, resourceObj);

        if (_currentTab == Tab.Select)
            SetSelectToggles(cellIndex);
    }

    private void ChangeTileBase(int cellIndex)
    {
        Vector3 cellPosition = HexGrid.GetCellWorldPosition(cellIndex);

        for (int i = 0; i < _basesPlaced.Length; i++)
        {
            // Skip if base is placed
            if (_basesPlaced[i] != cellIndex) continue;

            // If a base is not placed set to -1
            _basesPlaced[i] = -1;
        }

        // Only one base should exist per type
        if (_basesPlaced[_baseIndex] != -1)
            HexGrid.RemoveCellBase(_basesPlaced[_baseIndex]);

        HexGrid.RemoveCellResource(cellIndex);
        HexGrid.SetCellBiome(cellIndex, (Biome)_baseIndex);
        HexGrid.SetCellBase(cellIndex, (Base)_baseIndex, BaseTextures[_baseIndex]);

        _basesPlaced[_baseIndex] = cellIndex;

        if (_currentTab == Tab.Select)
            SetSelectToggles(cellIndex);
    }


    public void SetBiome(int index)
    {
        _biomeIndex = index;
    }
    public void SetResource(int index)
    {
        _resourceIndex = index;
    }
    public void SetBase(int index)
    {
        _baseIndex = index;
    }


    public void SaveMap()
    {
        _options.SaveMap(InputField.text, HexGrid.GetHexCells(), TGS.cells, Dropdown, _basesPlaced);
    }

    public void LoadMap()
    {
        _options.LoadMap(InputField.text);
    }

    public void LoadCellData(List<HexCellData> cellData)
    {
        HexGrid.SetCells(cellData.Select(c => new HexCell(c)).ToArray());

        for (int i = 0; i < cellData.Count; i++)
        {
            _biomeIndex = (int)cellData[i].biome;
            _resourceIndex = (int)cellData[i].resource;
            _baseIndex = (int)cellData[i].faction;

            if (_baseIndex != -1)
            {
                ChangeTileBase(i);

                _basesPlaced[_baseIndex] = i;
            }
            else if (_biomeIndex != -1)
            {
                ChangeTileBiome(i);

                if (_resourceIndex == -1)
                    continue;

                ChangeTileResource(i);
            }
        }
    }

    public void SetDropdownScene(string sceneName)
    {
        int index = Extensions.SceneGetInt(sceneName);

        if (index == -1)
        {
            ErrorText.text = $"\"{sceneName}\" does not exist in build";
            Dropdown.value = 0;
            return;
        }

        Dropdown.value = index;
    }
}
