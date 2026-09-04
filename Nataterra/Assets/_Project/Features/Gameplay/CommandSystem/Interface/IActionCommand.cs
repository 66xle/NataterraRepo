using PurrNet;
using PurrNet.Packing;
using UnityEngine;

public interface IActionCommand : IPackedAuto
{
    public PlayerID PlayerID { get; set; }
}
