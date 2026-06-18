using System;
using System.Collections.Generic;

public class SceneInitialize : Singleton<SceneInitialize>
{
    private readonly SortedDictionary<int, List<Action>> _subscribers = new();

    /// <summary>
    /// Subscribe with optional priority (default = 0). Lower number runs first.
    /// </summary>
    public void Subscribe(Action callback, int priority = 0)
    {
        if (!_subscribers.ContainsKey(priority))
            _subscribers[priority] = new List<Action>();

        _subscribers[priority].Add(callback);
    }

    /// <summary>
    /// Unsubscribe from all priorities.
    /// </summary>
    public void Unsubscribe(Action callback)
    {
        foreach (var kv in _subscribers)
            kv.Value.Remove(callback);
    }

    /// <summary>
    /// Invoke all subscribers in priority order.
    /// </summary>
    public void Invoke()
    {
        foreach (var kv in _subscribers)
        {
            foreach (var cb in kv.Value)
                cb.Invoke();
        }
    }

    /// <summary>
    /// Clears all subscribers.
    /// </summary>
    public void Clear() => _subscribers.Clear();
}