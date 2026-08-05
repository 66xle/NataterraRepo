using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable/Unit Data", order = 2)]
public class UnitData : ScriptableObject
{
    [Header("Info")]
    public UnitType UnitType;
    public GameObject Prefab;
    public Sprite Sprite;
    public int StartingAvailiableUnits;

    [Header("Stats")]
    public int Attack;
    public int Armour;
    public int Health;
    public int Initiative;
    public int Group;
    public int Movement;

    [Header("Cost")]
    public int FoodCost;
    public int WoodCost;
    public int MetalCost;

    [Header("Traits")]
    public bool IsFlying;
}
