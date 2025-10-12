using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    [Header("Input's")]
    public PlayerControls _inputAction;

    [Header("Component's")]
    Animator _anim;

    [Header("Bool's")]
    public bool _axe;

    void Awake()
    {
        _inputAction = new PlayerControls();
        _anim = GetComponent<Animator>();
    }

    #region Enable & Disable

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

    #endregion

    private void OnLeftClick(InputAction.CallbackContext _ctx)
    {
        if(_axe)
        {
            _anim.SetTrigger("Axe");
            PlayerMovement.moveSpeed = 0;
        }
    }
}
