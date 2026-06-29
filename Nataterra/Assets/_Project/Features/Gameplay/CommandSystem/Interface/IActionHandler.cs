using UnityEngine;

public interface IActionHandler<T> where T : IActionCommand
{
    void Handle(T command);
}

