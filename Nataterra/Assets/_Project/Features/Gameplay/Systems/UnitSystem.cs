using PurrNet;
using System.Collections.Generic;
using UnityEngine;

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
    public void SpawnUnitToAll(List<UnitType> types, List<string> GUIDs, int cellIndex, List<StateChange> changes)
    {
        _gs.SetLocalChanges(changes);

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
        _gs.MSM.AddUnitObject(GUID, newUnit);
    }
    

    [ObserversRpc]
    public void MoveUnit(List<string> GUIDs, int destination, List<StateChange> changes)
    {
        _gs.SetLocalChanges(changes);

        foreach (string guid in GUIDs)
        {
            GameObject moveUnit = _gs.MSM.GetUnitObject(guid);

            moveUnit.transform.position = _gs.TGS.CellGetPosition(destination);
        }
    }
}
