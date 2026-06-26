using TGS;
using UnityEngine;

public class GameplaySystem : MonoBehaviour
{
    public GameObject UnitPrefab;

    public UnitSystem UnitSystem { get; private set; }


    private void Start()
    {
        UnitSystem = new UnitSystem();
    }
}
