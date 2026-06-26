using UnityEngine;

public interface IActionCommandHandler<T> where T : IActionCommand
{
    void Handle(T command);
}

