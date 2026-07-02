using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable/Unit Data", order = 2)]
public class UnitData : ScriptableObject
{
    public UnitType Unit;
    public GameObject Prefab;

    public int Attack;
    public int Armour;
    public int Health;
    public int Initiative;
    public int Group;
    public int Movement;
}
