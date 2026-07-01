using System.Collections.Generic;
using UnityEngine;

public class UnitSystem : MonoBehaviour
{
    GameplaySystem _gs;

    private void Start()
    {
        _gs = GetComponentInParent<GameplaySystem>();
    }


    public void SpawnUnit(List<GameObject> prefabs, int cellIndex, List<StateChange> changes)
    {
        _gs.SetLocalChanges(changes);

        foreach (GameObject unitObj in prefabs)
        {
            Vector3 spawnPosition = _gs.TGS.CellGetPosition(cellIndex);

            Instantiate(unitObj, spawnPosition, Quaternion.identity);
        }
    }
}
