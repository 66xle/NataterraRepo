using System.Collections.Generic;
using UnityEngine;

public class HexCellState
{
    public Biome Biome = Biome.None;
    public Resource Resource = Resource.None;
    public Base Faction = Base.None;

    public List<Group> listOfGroups;

    public HexCellState(HexCellData cell)
    {
        Biome = cell.biome;
        Resource = cell.resource;
        Faction = cell.faction;

        listOfGroups = new();
    }
}
