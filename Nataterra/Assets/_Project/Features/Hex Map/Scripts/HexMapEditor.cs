using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public GameObject prefab;
    public Vector3 resourceScale = Vector3.one;
    public Vector3 baseScale = new Vector3(1.5f, 1.5f, 1.5f);

    public LayerMask hexGridLayer;

    public HexGrid hexGrid;



    int[] basesPlaced = new int[4];

    Tab currentTab;

    int biomeIndex;
    int resourceIndex;
    int baseIndex;

    void Awake()
    {
        SelectButton(0);
        biomeIndex = 0;
        resourceIndex = 0;
        baseIndex = 0;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && !EventSystem.current.IsPointerOverGameObject())
        {
            ResetCell();
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, hexGridLayer))
        {
            int cellIndex = hexGrid.GetCellIndex(hit.point);

            if (Tab.Biome == currentTab)
            {
                hexGrid.SetCellBiome(cellIndex, (Biome)biomeIndex, biomeColors[biomeIndex]);
            }
            else if (Tab.Resource == currentTab)
            {
                Vector3 cellPosition = hexGrid.GetCellPosition(cellIndex);
                cellPosition = hexGrid.transform.TransformPoint(cellPosition);

                GameObject resourceObj = Instantiate(prefab, cellPosition, Quaternion.identity);
                resourceObj.transform.localScale = resourceScale;
                resourceObj.GetComponent<Renderer>().material.color = resourceColors[resourceIndex];


                hexGrid.RemoveCellBase(cellIndex);
                hexGrid.SetCellResource(cellIndex, (Resource)resourceIndex, resourceObj);
            }
            else if (Tab.Base == currentTab)
            {
                Vector3 cellPosition = hexGrid.GetCellPosition(cellIndex);
                cellPosition = hexGrid.transform.TransformPoint(cellPosition);

                GameObject baseObj = Instantiate(prefab, cellPosition, Quaternion.identity);
                baseObj.transform.localScale = baseScale;
                baseObj.GetComponent<Renderer>().material.color = Color.white;

                // Only one base should exist per type
                hexGrid.RemoveCellBase(basesPlaced[baseIndex]);

                hexGrid.RemoveCellResource(cellIndex);
                hexGrid.SetCellBiome(cellIndex, (Biome)baseIndex, biomeColors[baseIndex]);
                hexGrid.SetCellBase(cellIndex, baseIndex, baseObj);

                basesPlaced[baseIndex] = cellIndex;
            }
        }
    }

    void ResetCell()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, hexGridLayer))
        {
            int cellIndex = hexGrid.GetCellIndex(hit.point);

            hexGrid.RemoveCellBiome(cellIndex);
            hexGrid.RemoveCellResource(cellIndex);
            hexGrid.RemoveCellBase(cellIndex);
        }
    }

    public void SelectButton(int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == index)
            {
                buttons[i].interactable = false;
                panels[i].SetActive(true);
                currentTab = (Tab)index;
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
