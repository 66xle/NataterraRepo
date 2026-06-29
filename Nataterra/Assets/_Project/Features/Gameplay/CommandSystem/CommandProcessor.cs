using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor
{
    private Dictionary<Type, Action<IActionCommand>> _handlers = new();

    public void Register<T>(IActionHandler<T> handler) where T : IActionCommand
    {
        _handlers.Add(typeof(T), command => handler.Handle((T)command));
    }

    public void Process(IActionCommand command)
    {
        if (_handlers.TryGetValue(command.GetType(), out Action<IActionCommand> action))
        {
            action(command);
        }
    }
}
