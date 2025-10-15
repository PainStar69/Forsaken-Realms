using UnityEditor.ShaderGraph;
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
    public bool _pickaxe;

    [Header("Float's")]
    public float _detectDistance = 1f;

    [Header("LayerMask's")]
    public LayerMask _objectLayer;

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
        _inputAction.Player.Inventory.performed += OnAlphaOneClick;
        _inputAction.Player.Inventory.performed += OnAlphaTwoClick;
        _inputAction.Player.Move.performed += OnMove;
        _inputAction.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        _inputAction.Player.Action.performed -= OnLeftClick;
        _inputAction.Player.Inventory.performed -= OnAlphaOneClick;
        _inputAction.Player.Inventory.performed -= OnAlphaTwoClick;
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
        else if(_pickaxe)
        {
            _anim.SetTrigger("Pickaxe");
            PlayerMovement.moveSpeed = 0;
        }
    }

    //Job: Changed and Remove
    private void OnAlphaOneClick(InputAction.CallbackContext _ctx)
    {
        var _control = _ctx.control;

        if (_control.name == "1")
        {
            _axe = true;
            _pickaxe = false;
        }
    }

    private void OnAlphaTwoClick(InputAction.CallbackContext _ctx)
    {
        var _control = _ctx.control;

        if (_control.name == "2")
        {
            _pickaxe = true;
            _axe = false;
        }
    }

    public void DetectObjectInFront()
    {
        Vector2 _rayOrigin = (Vector2)transform.position + _lookDirection * 0.3f;

        RaycastHit2D _hit = Physics2D.Raycast(_rayOrigin, _lookDirection, _detectDistance, _objectLayer);

        Debug.DrawRay(_rayOrigin, _lookDirection * _detectDistance, Color.red, 0.1f);

        if (_hit.collider != null && _hit.collider.CompareTag("Tree") && _axe == true)
        {
            ParticleSystem _treeParticle = _hit.collider.transform.parent.Find("TreeParticle").gameObject.GetComponent<ParticleSystem>();

            _hit.collider.transform.parent.gameObject.GetComponent<Shake>().ShakeStart();

            _hit.collider.transform.parent.Find("TreeChop").gameObject.GetComponent<ObjectManager>().ObjectTakeDamage(1);
            _treeParticle.Play();

            if(_hit.collider.transform.parent.Find("TreeChop").gameObject.GetComponent<ObjectManager>()._objectHealth < 1)
            {
                Destroy(_treeParticle.gameObject);
            }

            if (_hit.collider.transform.parent.Find("TreeChop").gameObject.GetComponent<ObjectManager>()._objectHealth <= 0)
            {
                _hit.collider.transform.parent.Find("TreeChop").gameObject.GetComponent<Animator>().SetTrigger("Chop"); 
            }

            _audioSource.clip = _clips[0];
            _audioSource.Play();
        }
        else if(_hit.collider != null && _hit.collider.CompareTag("Rock") && _pickaxe == true)
        {
            _hit.collider.transform.parent.gameObject.GetComponent<Shake>().ShakeStart();

            _hit.collider.transform.parent.gameObject.GetComponent<ObjectManager>().ObjectTakeDamage(1);

            _audioSource.clip = _clips[1];
            _audioSource.Play();
        }
    }
}
