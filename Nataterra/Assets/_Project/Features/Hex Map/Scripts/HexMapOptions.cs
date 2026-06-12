using System.Collections.Generic;
using TGS;
using TMPro;
using UnityEditor;
using UnityEngine;

public class HexMapOptions
{
    HexMapEditor editor;

    public HexMapOptions(HexMapEditor editor)
    {
        this.editor = editor; 
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

        MapData loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/_Project/Features/Hex Map/ScriptableObjects/{fileName}.asset", typeof(MapData)) as MapData;

        if (loadedAsset != null)
        {
            editor.errorText.text = "Name already exists";
            return;
        }

        editor.errorText.text = $"Created \"{fileName}\"";
        editor.errorText.color = Color.green;

        MapData mapData = new(cells, tgsCells);
        AssetDatabase.CreateAsset(mapData, $"Assets/_Project/Features/Hex Map/ScriptableObjects/{fileName}.asset");
    }

    public void LoadMap()
    {

    }


}
