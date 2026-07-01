using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable/Unit Data", order = 2)]
public class UnitData : ScriptableObject
{
    public UnitType Unit;
    public GameObject Prefab;
}
