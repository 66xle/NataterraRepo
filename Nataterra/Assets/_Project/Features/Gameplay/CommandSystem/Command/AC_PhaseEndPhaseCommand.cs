using PurrNet;
using UnityEngine;

public struct AC_PhaseEndPhaseCommand : IActionCommand
{
    public PlayerID PlayerID { get; set; }
    public DevelopmentCart Cart;
}
