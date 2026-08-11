using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DijkstraResult
{
    public Dictionary<int, int> Cost = new();
    public Dictionary<int, int> Parent = new();

    public List<int> BuildPath(int destination)
    {
        List<int> path = new();

        if (!Cost.ContainsKey(destination))
            return path;


        HashSet<int> visited = new();
        int current = destination;

        while (current != -1)
        {
            if (!visited.Add(current))
            {
                Debug.LogError($"Dijkstra path contains a cycle! " + $"Cell {current} was already visited.");

                path.Clear();
                return path;
            }

            path.Add(current);

            if (!Parent.TryGetValue(current, out int parent))
            {
                Debug.LogError($"Dijkstra path is missing a parent for cell {current}.");

                path.Clear();
                return path;
            }

            current = parent;
        }

        path.Reverse();

        return path;
    }

    public int GetDestinationCost(int destination)
    {
        return Cost[destination];
    }


    public List<int> GetIndexList()
    {
        return Cost.Keys.ToList();
    }

    public bool Contains(int index)
    {
        return Cost.ContainsKey(index);
    }
}
