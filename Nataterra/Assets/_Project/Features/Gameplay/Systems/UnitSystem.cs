using TGS;
using UnityEngine;

public class UnitSystem : GameplaySystem
{

    public void RecruitUnit(Base faction, int amount, Unit unit, int cellIndex)
    {
        Vector3 spawnPosition = _tgs.CellGetPosition(cellIndex);

        Instantiate(UnitPrefab, spawnPosition, Quaternion.identity);
    }
}
