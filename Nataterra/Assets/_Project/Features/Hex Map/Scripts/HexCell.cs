using TGS;
using UnityEngine;

public class HexCell
{
    public Cell cell;

    public Color color;
    public Biome biome = Biome.None;

    public Resource resource = Resource.None;
    public GameObject resourceObj;

    public Base raceBase = Base.None;
    public GameObject baseObj;

    public HexCell(Cell cell)
    {
        this.cell = cell;
    }
}
