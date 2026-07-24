using PurrNet;
using UnityEngine;

public struct AC_PhaseEndPhaseCommand : IActionCommand
{
    public PlayerID ID { get; set; }
    public GameplayState CurrentState;
    public DevelopmentCart Cart;
}
