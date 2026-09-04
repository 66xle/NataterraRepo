using PurrNet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1f, -1);
        newUnit.GetComponent<NavMeshAgent>().Warp(hit.position);

        _gs.MSM.AddUnitObject(GUID, newUnit);
    }

    [ObserversRpc]
    public void MoveUnit(List<string> GUIDs, int destination)
    {
        foreach (string guid in GUIDs)
        {
            GameObject moveUnit = _gs.MSM.GetUnitObject(guid);

            Vector3 cellPos = _gs.TGS.CellGetPosition(destination);
            NavMesh.SamplePosition(cellPos, out NavMeshHit hit, 1f, -1);

            moveUnit.GetComponent<NavMeshAgent>().SetDestination(hit.position);
        }
    }
}
