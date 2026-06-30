using UnityEngine;

public class UnitSystem : MonoBehaviour
{
    GameplaySystem _gs;

    private void Start()
    {
        _gs = GetComponentInParent<GameplaySystem>();
    }



    public void RecruitUnit(int amount, Unit unit, int cellIndex)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = _gs.TGS.CellGetPosition(cellIndex);

            Instantiate(_gs.UnitPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
