using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TGS;

public enum Tab
{
    Select = 0,
    Biome = 1,
    Resource = 2,
    Base = 3
}


public class HexMapEditor : MonoBehaviour
{
    public Color[] biomeColors;
    public Color[] resourceColors;

    public List<Button> buttons;

    public List<GameObject> panels;

    public TMP_Text selectedTileText;
    public Toggle disableToggle;

    public GameObject prefab;
    public Vector3 resourceScale = Vector3.one;
    public Vector3 baseScale = new Vector3(1.5f, 1.5f, 1.5f);

    public LayerMask hexGridLayer;

    public HexGrid hexGrid;

    int[] basesPlaced = new int[4] { -1, -1, -1, -1 };
    ToggleGroup[] selectToggleGroups = new ToggleGroup[3];

    Tab currentTab;

    int currentTileIndex;
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

        hexGrid.tgs.OnCellMouseDown += OnCellMouseDown;
    }

    void OnCellMouseDown(TerrainGridSystem grid, int cellIndex, int buttonIndex)
    {
        if (buttonIndex == 0)
        {
            if (Tab.Select == currentTab)
            {
                //ShowTileDetails(cellIndex, hit.point);
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
        else if (buttonIndex == 1)
        {
            hexGrid.RemoveCellBiome(cellIndex);
            hexGrid.RemoveCellResource(cellIndex);
            hexGrid.RemoveCellBase(cellIndex);
        }
    }


    private void ShowTileDetails(int cellIndex, Vector3 position)
    {
        currentTileIndex = cellIndex;
        panels[0].SetActive(true);

        HexCoordinates coordinates = hexGrid.GetHexCoordinates(position);
        selectedTileText.text = $"Selected: {coordinates.X}, {coordinates.Z}";

        Biome biome = hexGrid.GetCellBiome(cellIndex);
        Resource resource = hexGrid.GetCellResource(cellIndex);
        Base raceBase = hexGrid.GetCellBase(cellIndex);

        selectToggleGroups[0].SetAllTogglesOff();
        selectToggleGroups[1].SetAllTogglesOff();
        selectToggleGroups[2].SetAllTogglesOff();

        if (Biome.None != biome)
            selectToggleGroups[0].GetComponentsInChildren<Toggle>()[(int)biome].isOn = true;

        if (Resource.None != resource)
            selectToggleGroups[1].GetComponentsInChildren<Toggle>()[(int)resource].isOn = true;

        if (Base.None != raceBase)
            selectToggleGroups[2].GetComponentsInChildren<Toggle>()[(int)raceBase].isOn = true;
    }


    public void ChangeTileType(int typeIndex)
    { 
        if (typeIndex == 0)
        {
            ChangeTileBase(currentTileIndex);
        }
        else if (typeIndex == 1)
        {
            ChangeTileResource(currentTileIndex);
        }
        else if (typeIndex == 2)
        {
            ChangeTileBase(currentTileIndex);
        }
        else
        {
            Debug.LogError("Incorrect TypeIndex: " + typeIndex);
        }
    }

    private void ChangeTileBiome(int cellIndex)
    {
        hexGrid.SetCellBiome(cellIndex, (Biome)biomeIndex, biomeColors[biomeIndex]);
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
    }


    public void DisableToggle()
    {
        bool isDisabled = disableToggle.isOn;

        selectToggleGroups[0].gameObject.SetActive(isDisabled);
        selectToggleGroups[1].gameObject.SetActive(isDisabled);
        selectToggleGroups[2].gameObject.SetActive(isDisabled);
    }

    public void SelectButton(int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == index)
            {
                buttons[i].interactable = false;
                currentTab = (Tab)index;

                if (i > 0)
                    panels[i].SetActive(true);
            }
            else
            {
                buttons[i].interactable = true;
                panels[i].SetActive(false);
            }
        }
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
