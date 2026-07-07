using PurrNet;
using System.Collections.Generic;
using UnityEngine;

public class UnitSystem : MonoBehaviour
{
    GameplaySystem _gs;

    private void Start()
    {
        _gs = GetComponentInParent<GameplaySystem>();
    }

    public void SpawnUnit(List<GameObject> prefabs, List<string> GUIDs, int cellIndex, List<StateChange> changes)
    {
        _gs.SetLocalChanges(changes);

        for (int i = 0; i < prefabs.Count; i++)
        {
            Vector3 spawnPosition = _gs.TGS.CellGetPosition(cellIndex);

            GameObject newUnit = Instantiate(prefabs[i], spawnPosition, Quaternion.identity);

            _gs.MSM.AddUnitObject(GUIDs[i], newUnit);
        }
    }

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
