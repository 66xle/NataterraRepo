using UnityEngine;

public class AH_PhaseHandler : IActionHandler<AC_PhaseEndPhaseCommand>
{
    GameplaySystem _gs;
    ServerMapWrapper _map;

    public AH_PhaseHandler(GameplaySystem gs, ServerMapWrapper map)
    {
        _gs = gs;
        _map = map;
    }

    public void Handle(AC_PhaseEndPhaseCommand command)
    {
        if (command.PlayerID != _map.CurrentPlayerTurn)
            return;

        GameplayState state = command.CurrentState;

        if (command.CurrentState == GameplayState.MovementPhase)
        {
            EndMovementPhase();
            state = GameplayState.ResourcePhase;
        }
        else if (command.CurrentState == GameplayState.ResourcePhase)
        {
            EndResourcePhase();
            state = GameplayState.CombatPhase;
        }
        else if (command.CurrentState == GameplayState.CombatPhase)
        {
            EndCombatPhase();
            state = GameplayState.DevelopmentPhase;
        }
        else if (command.CurrentState == GameplayState.DevelopmentPhase)
        {
            EndDevelopmentPhase();
            state = GameplayState.WaitingForTurn;
        }

        // Set server's state
        _map.SetPhaseState(state);
        _gs.MSM.EndPhaseForClient(command.PlayerID);
    }

    void EndMovementPhase()
    {

    }

    void EndResourcePhase()
    {

    }

    void EndCombatPhase()
    {

    }

    void EndDevelopmentPhase()
    {

    }
}
