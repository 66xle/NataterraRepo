using System.Collections.Generic;
using PurrNet;
using TGS;

public class GameplaySystem : NetworkBehaviour
{
    public MapStateMachine MSM;

    public UnitSystem UnitSystem { get; private set; }

    public TerrainGridSystem TGS { get; private set; }


    private void OnEnable()
    {
        SceneInitialize.Instance.Subscribe(Init);
    }

    private void OnDisable()
    {
        SceneInitialize.Instance.Unsubscribe(Init);
    }

    private void Init()
    {
        UnitSystem = GetComponentInChildren<UnitSystem>();

        TGS = TerrainGridSystem.instance;
    }

    [ObserversRpc(bufferLast:true)]
    public void SetLocalChanges(List<StateChange> changes)
    {
        foreach (StateChange state in changes)
        {
            MSM.SetCellState(state.State, state.CellIndex);
        }
    }


    public DijkstraResult CalculateMovementRange(int startCell, int maxMovement)
    {
        DijkstraResult result = new DijkstraResult();

        List<int> open = new();

        open.Add(startCell);

        result.Cost[startCell] = 0;
        result.Parent[startCell] = -1;

        while (open.Count > 0)
        {
            // Find the node with the lowest cost
            int current = open[0];

            for (int i = 1; i < open.Count; i++)
            {
                if (result.Cost[open[i]] < result.Cost[current])
                    current = open[i];
            }

            open.Remove(current);

            int currentCost = result.Cost[current];

            Cell cell = TGS.cells[current];

            foreach (Cell neighbour in cell.neighbours)
            {
                if (!CanEnter(neighbour))
                    continue;

                int newCost = currentCost + GetMovementCost(neighbour);

                if (newCost > maxMovement)
                    continue;

                if (!result.Cost.TryGetValue(neighbour.index, out int oldCost) ||
                    newCost < oldCost)
                {
                    result.Cost[neighbour.index] = newCost;
                    result.Parent[neighbour.index] = current;

                    if (!open.Contains(neighbour.index))
                        open.Add(neighbour.index);
                }
            }
        }

        return result;
    }

    private bool CanEnter(Cell cell)
    {
        if (!cell.canCross)
            return false;

        return true;
    }

    private int GetMovementCost(Cell cell)
    {
        // Check if ground or flying type

        // If ground check if cell is a mountain

        return 1;
    }

    
}
