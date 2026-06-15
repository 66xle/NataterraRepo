using System.Collections.Generic;
using System.Linq;
using TGS;
using TMPro;
using UnityEditor;
using UnityEngine;

public class HexMapOptions
{
    HexMapEditor editor;

    string path;

    public HexMapOptions(HexMapEditor editor)
    {
        this.editor = editor;
        path = "Assets/_Project/Features/Hex Map/ScriptableObjects";
    }


    public void SaveMap(string fileName, HexCell[] cells, List<Cell> tgsCells)
    {
        editor.errorText.text = "";
        editor.errorText.color = Color.red;

        if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
        {
            editor.errorText.text = "Field is empty";
            return;
        }

        MapData loadedAsset = AssetDatabase.LoadAssetAtPath($"{path}/{fileName}.asset", typeof(MapData)) as MapData;

        if (loadedAsset != null)
        {
            editor.errorText.text = "Name already exists";
            return;
        }

        editor.errorText.text = $"Created \"{fileName}\"";
        editor.errorText.color = Color.green;

        MapData mapData = ScriptableObject.CreateInstance<MapData>();
        mapData.Initialize(cells, tgsCells);
        AssetDatabase.CreateAsset(mapData, $"{path}/{fileName}.asset");
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

        MapData loadedAsset = AssetDatabase.LoadAssetAtPath($"{path}/{fileName}.asset", typeof(MapData)) as MapData;

        if (loadedAsset == null)
        {
            editor.errorText.text = $"\"{fileName}\" doesn't exist";
            return;
        }


        editor.errorText.text = $"Loaded \"{fileName}\"";
        editor.errorText.color = Color.green;


        HexCell[] cells = loadedAsset.hexCells.Select(x => new HexCell(x)).ToArray();
        List<Cell> tgsCells = loadedAsset.tgsCells.Select(x => new Cell(x)).ToList();

        editor.hexGrid.RegenerateGrid(cells, tgsCells);
    }

    public void LoadResources()
    {

    }
}
