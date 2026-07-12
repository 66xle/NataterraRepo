using System.Collections.Generic;
using UnityEngine;

public class HexCellState
{
    public Biome Biome = Biome.None;
    public Resource Resource = Resource.None;
    public Base Faction = Base.None;

    public Dictionary<UnitType, Group> DictOfGroups;

    public HexCellState(HexCellData cell)
    {
        Biome = cell.biome;
        Resource = cell.resource;
        Faction = cell.faction;

        DictOfGroups = new();
    }

    public HexCellState(HexCellState state)
    {
        Biome = state.Biome;
        Resource = state.Resource;
        Faction = state.Faction;

        DictOfGroups = state.DictOfGroups;
    }
}
