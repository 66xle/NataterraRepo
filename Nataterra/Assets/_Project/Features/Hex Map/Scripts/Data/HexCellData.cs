using System;
using UnityEngine;

[Serializable]
public class HexCellData
{
    public Biome biome = Biome.None;
    public Resource resource = Resource.None;
    public Base faction = Base.None;

    public HexCellData(HexCell cell)
    {
        biome = cell.biome;
        resource = cell.resource;
        faction = cell.faction;
    }
}
