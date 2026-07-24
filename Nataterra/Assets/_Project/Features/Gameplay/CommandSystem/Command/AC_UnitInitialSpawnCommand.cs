using UnityEngine;
using PurrNet;

public struct AC_UnitInitialSpawnCommand : IActionCommand
{
    public PlayerID ID { get; set; }
}
