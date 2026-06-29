using TGS;
using UnityEngine;

public class GameplaySystem : MonoBehaviour
{
    public GameObject UnitPrefab;

    public UnitSystem UnitSystem { get; private set; }


    protected TerrainGridSystem _tgs;


    private void Start()
    {
        UnitSystem = new UnitSystem();
        _tgs = TerrainGridSystem.instance;
    }
}
