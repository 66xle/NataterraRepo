using PurrNet;
using System.Collections.Generic;
using System.IO;
using TGS;
using UnityEngine;
using UnityEngine.AI;
using VisualPath;

public class UnitSystem : NetworkBehaviour
{
    GameplaySystem _gs;

    private void Awake()
    {
        _gs = GetComponentInParent<GameplaySystem>();
    }


    [TargetRpc]
    public void SpawnUnitToClient(PlayerID target, List<UnitType> types, List<string> GUIDs, List<int> indexs)
    {
        for (int i = 0; i < types.Count; i++)
        {
            SpawnUnit(types[i], GUIDs[i], indexs[i]);
        }
    }


    [ObserversRpc]
    public void SpawnUnitToAll(List<UnitType> types, List<string> GUIDs, int cellIndex)
    {
        for (int i = 0; i < types.Count; i++)
        {
            SpawnUnit(types[i], GUIDs[i], cellIndex);
        }
    }
    
    void SpawnUnit(UnitType type, string GUID, int cellIndex)
    {
        Vector3 spawnPosition = _gs.TGS.CellGetPosition(cellIndex);
        GameObject prefab = _gs.MSM.GetUnitPrefab(type);

        GameObject newUnit = Instantiate(prefab, spawnPosition, Quaternion.identity);
        newUnit.GetComponent<UnitAI>().SetPosition(spawnPosition);

        _gs.MSM.AddUnitObject(GUID, newUnit);
    }

    [ObserversRpc]
    public async void MoveUnit(List<string> GUIDs, List<VisualHex> listHexes, Vector3 start, Vector3 end)
    {
        List<Vector3> path = await _gs.PathSystem.GeneratePath(listHexes, start, end);
        if (path == null) return;

        foreach (string guid in GUIDs)
        {
            GameObject moveUnit = _gs.MSM.GetUnitObject(guid);
            moveUnit.GetComponent<UnitAI>().SetPath(path);
        }
    }
}
