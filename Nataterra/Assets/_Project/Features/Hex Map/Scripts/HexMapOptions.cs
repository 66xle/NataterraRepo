using System.Collections.Generic;
using System.IO;
using System.Linq;
using TGS;
using TMPro;
using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;

public class HexMapOptions
{
    HexMapEditor editor;

    string savePath;

    public HexMapOptions(HexMapEditor editor)
    {
        this.editor = editor;
        savePath = "Assets/_Project/Features/Hex Map/JsonData";
    }


    public void SaveMap(string fileName, HexCell[] cells, List<Cell> tgsCells, TMP_Dropdown dropdown)
    {
        editor.errorText.text = "";
        editor.errorText.color = Color.red;

        if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
        {
            editor.errorText.text = "Field is empty";
            return;
        }

        string path = $"{savePath}/{fileName}.json";
        if (File.Exists(path))
        {
            editor.errorText.text = "Name already exists";
            return;
        }

        if (dropdown.value == 0)
        {
            editor.errorText.text = $"Please set a valid scene!";
            return;
        }

        editor.errorText.text = $"Created \"{fileName}\"";
        editor.errorText.color = Color.green;

        string sceneName = dropdown.options[dropdown.value].text;
        MapData mapData = new(cells, tgsCells, sceneName);

        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(path, json);

        AssetDatabase.Refresh();
    }

    public void LoadMap(string fileName)
    {
        editor.errorText.text = "";
        editor.errorText.color = Color.red;

        if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
        {
            editor.errorText.text = "Field is empty";
            return;
        }

        string path = $"{savePath}/{fileName}.json";

        if (!File.Exists(path))
        {
            editor.errorText.text = $"\"{fileName}\" doesn't exist";
            return;
        }

        editor.errorText.text = $"Loaded \"{fileName}\"";
        editor.errorText.color = Color.green;


        string json = File.ReadAllText(path);
        MapData mapData = JsonUtility.FromJson<MapData>(json);

        ResetCells(mapData.hexCells.Count);

        editor.hexGrid.RegenerateGrid(mapData.tgsCells);
        editor.LoadCellData(mapData.hexCells);
        editor.SetDropdownScene(mapData.sceneName);
    }

    public void ResetCells(int count)
    {
        for (int i = 0; i < count; i++)
        {
            editor.ResetCell(i);
        }
    }

}
