using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    [Header("Input's")]
    public PlayerControls _inputAction;

    [Header("Component's")]
    Animator _anim;
    AudioSource _audioSource;

    [Header("MP3")]
    public AudioClip[] _clips;

    [Header("Bool's")]
    public bool _axe;

    [Header("Float's")]
    public float _detectDistance = 1f;

    [Header("LayerMask's")]
    public LayerMask _treeLayer;

    [Header("Vector's")]
    private Vector2 _lookDirection = Vector2.down;
    private Vector2 _moveInput;

    void Awake()
    {
        _inputAction = new PlayerControls();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _inputAction.Enable();
        _inputAction.Player.Action.performed += OnLeftClick;
        _inputAction.Player.Move.performed += OnMove;
        _inputAction.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        _inputAction.Player.Action.performed -= OnLeftClick;
        _inputAction.Player.Move.performed -= OnMove;
        _inputAction.Player.Move.canceled -= OnMoveCanceled;
        _inputAction.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        if (_moveInput.sqrMagnitude > 0.01f)
            _lookDirection = _moveInput.normalized;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    private void OnLeftClick(InputAction.CallbackContext _ctx)
    {
        if (_axe)
        {
            _anim.SetTrigger("Axe");
            PlayerMovement.moveSpeed = 0;
        }
    }

    public void DetectTreeInFront()
    {
        // ýþýnýn baþlangýç noktasý karakterin merkezinden deðil, bakýþ yönünde biraz ileriden baþlýyor
        Vector2 rayOrigin = (Vector2)transform.position + _lookDirection * 0.3f;

        // Ray çiz
        RaycastHit2D _hit = Physics2D.Raycast(rayOrigin, _lookDirection, _detectDistance, _treeLayer);

        // Debug için (Unity Scene’de görmen için)
        Debug.DrawRay(rayOrigin, _lookDirection * _detectDistance, Color.red, 0.1f);

        if (_hit.collider != null && _hit.collider.CompareTag("Tree"))
        {
            _hit.collider.transform.parent.gameObject.GetComponent<Shake>().ShakeStart();
            _audioSource.clip = _clips[0];
            _audioSource.Play();
        }
    }
}
