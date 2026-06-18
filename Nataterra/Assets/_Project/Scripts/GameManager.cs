using System.IO;
using UnityEditor;
using UnityEditorInternal.VR;
using UnityEngine;
using TGS;


public class GameManager : Singleton<GameManager>
{
    public string jsonFileName;

    MapData _mapData;

    public MapData MapData { get { return _mapData; } }



    void Start()
    {
        LoadMapData();
    }


    public void LoadMapData()
    {
        if (string.IsNullOrEmpty(jsonFileName) || string.IsNullOrWhiteSpace(jsonFileName))
        {
            Debug.LogError("GameManager: File name is empty");
            return;
        }

        string path = $"Assets/_Project/Features/Hex Map/JsonData/{jsonFileName}.json";
        if (!File.Exists(path))
        {
            Debug.LogError($"GameManager: JsonData \"{jsonFileName}\" does not exist");
            return;
        }

        string json = File.ReadAllText(path);
        MapData mapData = JsonUtility.FromJson<MapData>(json);
        _mapData = mapData;
    }

}
