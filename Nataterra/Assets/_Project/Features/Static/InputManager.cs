using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting.ReorderableList;

public class InputManager : Singleton<InputManager>
{
    Vector2 _moveInput;

    public Vector2 MoveInput { get { return _moveInput; } }

    public Action OnLeftClickEvent;
    public Action OnRightClickEvent;


    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnLeftClickEvent?.Invoke();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnRightClickEvent?.Invoke();
    }
}
