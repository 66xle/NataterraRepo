using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : Singleton<InputManager>
{
    Vector2 _moveInput;

    public Vector2 MoveInput { get { return _moveInput; } }

    public Action OnClickEvent;


    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnClickEvent?.Invoke();
    }
}
