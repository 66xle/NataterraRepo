using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public Color color;
    public Biome biome = Biome.None;

    public Resource resource = Resource.None;
    public GameObject resourceObj;

    public Base raceBase = Base.None;
    public GameObject baseObj;
}
