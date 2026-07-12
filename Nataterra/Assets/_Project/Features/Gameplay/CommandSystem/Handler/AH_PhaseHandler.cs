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

        }
        else if (command.CurrentState == GameplayState.CombatPhase)
        {

        }
        else if (command.CurrentState == GameplayState.DevelopmentPhase)
        {

        }

        // Set server's state
        _map.SetPhaseState(state);
        _gs.MSM.EndPhaseForClient(command.PlayerID);

        // Show Next Phase UI to All Clients
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
