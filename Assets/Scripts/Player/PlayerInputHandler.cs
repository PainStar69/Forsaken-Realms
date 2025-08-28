using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerMovement movement;

    private void Awake()
    {
        controls = new PlayerControls();
        movement = GetComponent<PlayerMovement>();

        controls.Player.Move.performed += movement.OnMove;
        controls.Player.Move.canceled += movement.OnMove;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
