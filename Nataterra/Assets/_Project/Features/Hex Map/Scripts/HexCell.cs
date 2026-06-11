using TGS;
using UnityEngine;

public class HexCell
{
    public Cell cell;

    public Biome biome = Biome.None;

    public Resource resource = Resource.None;
    public GameObject resourceObj;

    public Base faction = Base.None;
    
    public HexCell(Cell cell)
    {
        this.cell = cell;
    }
}
