using System.Collections.Generic;
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

        int current = destination;

        while (current != -1)
        {
            path.Add(current);
            current = Parent[current];
        }

        path.Reverse();

        return path;
    }
}
