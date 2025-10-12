using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    public PlayerControls _inputAction;

    void Awake()
    {
        _inputAction = new PlayerControls();
    }

    private void OnEnable()
    {
        _inputAction.Enable();
        _inputAction.Player.Action.performed += OnLeftClick;
    }

    private void OnDisable()
    {
        _inputAction.Player.Action.performed -= OnLeftClick;
        _inputAction.Disable();
    }

    private void OnLeftClick(InputAction.CallbackContext _ctx)
    {
        
    }
}
