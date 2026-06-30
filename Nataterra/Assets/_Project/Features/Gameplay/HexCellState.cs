using UnityEngine;

public class HexCellState
{
    public Biome biome = Biome.None;
    public Resource resource = Resource.None;
    public Base faction = Base.None;

    public Group group;

    public HexCellState(HexCellData cell)
    {
        biome = cell.biome;
        resource = cell.resource;
        faction = cell.faction;
    }
}
