using PurrNet;
using TGS;
using UnityEngine;

public class GameplaySystem : NetworkBehaviour
{
    public GameObject UnitPrefab;

    public UnitSystem UnitSystem { get; private set; }

    public TerrainGridSystem TGS { get; private set; }


    private void OnEnable()
    {
        SceneInitialize.Instance.Subscribe(Init);
    }

    private void OnDisable()
    {
        SceneInitialize.Instance.Unsubscribe(Init);
    }

    private void Init()
    {
        UnitSystem = GetComponentInChildren<UnitSystem>();

        TGS = TerrainGridSystem.instance;
    }
}
