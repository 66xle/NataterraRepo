using PurrNet;
using UnityEngine;

public struct AC_PhaseEndPhaseCommand : IActionCommand
{
    public PlayerID PlayerID;
    public GameplayState CurrentState;
    public DevelopmentCart Cart;
}
