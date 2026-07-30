using PurrNet;
using UnityEngine;

public struct AC_PhaseEndPhaseCommand : IActionCommand
{
    public PlayerID ID { get; set; }
    public DevelopmentCart Cart;
}
