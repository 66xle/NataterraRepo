using TGS;
using UnityEngine;

public class UnitSystem : GameplaySystem
{
    TerrainGridSystem _tgs;

    private void Start()
    {
        _tgs = TerrainGridSystem.instance;
    }

    public void RecruitUnit(Base faction, int amount, Unit unit, int cellIndex)
    {
        Vector3 spawnPosition = _tgs.CellGetPosition(cellIndex);

        Instantiate(UnitPrefab, spawnPosition, Quaternion.identity);
    }
}
