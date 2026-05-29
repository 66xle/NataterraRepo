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
    public Color[] colors;

    public List<Button> buttons;

    public List<GameObject> panels;

    public HexGrid hexGrid;

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
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            int cellIndex = hexGrid.GetCellIndex(hit.point);

            if (Tab.Biome == currentTab)
            {
                hexGrid.ColorCell(cellIndex, colors[biomeIndex]);
                hexGrid.SetCellBiome(cellIndex, (Biome)biomeIndex);
            }
            else if (Tab.Resource == currentTab)
            {
                
            }
            else if (Tab.Base == currentTab)
            {
                
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
