using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Float's")]
    public static float moveSpeed = 7f;

    [Header("Component's")]
    private Rigidbody2D rb;
    private Animator _anim;

    [Header("Vector's")]
    public Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        float _speed = moveInput.sqrMagnitude;

        _anim.SetFloat("Speed", _speed);

        if (moveInput != Vector2.zero)
        {
            _anim.SetFloat("XInput", moveInput.x);
            _anim.SetFloat("YInput", moveInput.y);
        }
    }
}
