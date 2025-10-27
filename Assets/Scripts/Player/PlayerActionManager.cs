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
    public InventoryUI _inventoryUI;

    [Header("MP3")]
    public AudioClip[] _clips;

    [Header("Gameobject's")]
    public ParticleSystem _rockParticle;

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

        _inputAction.Player.Inventory.performed += OnAlphaButtonClick;

        _inputAction.Player.Move.performed += OnMove;
        _inputAction.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        _inputAction.Player.Action.performed -= OnLeftClick;

        _inputAction.Player.Inventory.performed -= OnAlphaButtonClick;

        _inputAction.Player.Move.performed -= OnMove;
        _inputAction.Player.Move.canceled -= OnMoveCanceled;

        _inputAction.Disable();
    }

    // Movement Input
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

    // Player Actions
    private void OnLeftClick(InputAction.CallbackContext _ctx)
    {
        if (_axe)
        {
            _anim.SetTrigger("Axe");
            PlayerMovement.moveSpeed = 0;
        }
        else if (_pickaxe)
        {
            _anim.SetTrigger("Pickaxe");
            PlayerMovement.moveSpeed = 0;
        }
    }

    private int _lastSelectedSlotIndex = -1;

    private void OnAlphaButtonClick(InputAction.CallbackContext _ctx)
    {
        var _control = _ctx.control;

        if (!int.TryParse(_control.name, out int num)) return;
        int _slotIndex = num - 1;

        if (_slotIndex < 0 || _slotIndex >= _inventoryUI._slotParents.Count) return;

        Transform slotTransform = _inventoryUI._slotParents[_slotIndex];

        // Eğer aynı slot tekrar tıklanıyorsa -> toggle: deselect et ve çık
        if (_lastSelectedSlotIndex == _slotIndex)
        {
            Animator sameAnim = slotTransform.GetComponent<Animator>();
            if (sameAnim != null)
            {
                sameAnim.SetTrigger("Deselect");
            }

            // istenirse aynı slot tekrar tıklanınca ekipman da kaldırılabilir:
            _axe = false;
            _pickaxe = false;

            _lastSelectedSlotIndex = -1;
            return;
        }

        // Farklı bir slot seçiliyorsa: önceki slotu deselect et
        if (_lastSelectedSlotIndex >= 0 && _lastSelectedSlotIndex < _inventoryUI._slotParents.Count)
        {
            Transform lastSlotTransform = _inventoryUI._slotParents[_lastSelectedSlotIndex];
            Animator lastAnimator = lastSlotTransform.GetComponent<Animator>();
            if (lastAnimator != null)
            {
                lastAnimator.SetTrigger("Deselect");
            }
        }

        // Yeni slotu select et
        Animator animator = slotTransform.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Select");
        }

        // Slot içeriğini kontrol et (mevcut mantığın aynısı)
        if (slotTransform.childCount > 0)
        {
            Transform _item = slotTransform.GetChild(0);
            string _itemName = _item.name;

            if (_itemName == "Axe")
            {
                _axe = true;
                _pickaxe = false;
            }
            else if (_itemName == "Pickaxe")
            {
                _pickaxe = true;
                _axe = false;
            }
            else
            {
                _axe = false;
                _pickaxe = false;
            }
        }
        else
        {
            _axe = false;
            _pickaxe = false;
        }

        // Son seçili olarak kaydet
        _lastSelectedSlotIndex = _slotIndex;
    }

    public void RecheckSlots()
    {
        for (int i = 0; i < _inventoryUI._slotParents.Count; i++)
        {
            Transform slotTransform = _inventoryUI._slotParents[i];

            if (slotTransform.childCount > 0)
            {
                Transform _item = slotTransform.GetChild(0);
                string _itemName = _item.name;

                if (_itemName == "Axe")
                {
                    _axe = true;
                    _pickaxe = false;
                }
                else if (_itemName == "Pickaxe")
                {
                    _pickaxe = true;
                    _axe = false;
                }
                else
                {
                    _axe = false;
                    _pickaxe = false;
                }
            }
            else
            {
                _axe = false;
                _pickaxe = false;
            }
        }
    }

    // Object Detection
    public void DetectObjectInFront()
    {
        Vector2 _rayOrigin = (Vector2)transform.position + _lookDirection * 0.3f;

        RaycastHit2D _hit = Physics2D.Raycast(_rayOrigin, _lookDirection, _detectDistance, _objectLayer);

        Debug.DrawRay(_rayOrigin, _lookDirection * _detectDistance, Color.red, 0.1f);

        if (_hit.collider != null)
        {
            if (_hit.collider.CompareTag("Tree") && _axe)
            {
                HandleTreeHit(_hit);
            }
            else if (_hit.collider.CompareTag("Rock") && _pickaxe)
            {
                HandleRockHit(_hit);
            }
        }
    }

    // Private helpers
    private void HandleTreeHit(RaycastHit2D _hit)
    {
        var parent = _hit.collider.transform.parent;

        ParticleSystem _treeParticle = parent.Find("TreeParticle").GetComponent<ParticleSystem>();
        parent.GetComponent<Shake>().ShakeStart();

        var treeChop = parent.Find("TreeChop").GetComponent<ObjectManager>();

        if (treeChop._objectHealth <= 1)
        {
            Destroy(_treeParticle.gameObject);
        }
        else
        {
            _treeParticle.Play();
        }

        treeChop.ObjectTakeDamage(1);

        if (treeChop._objectHealth <= 0)
        {
            treeChop.GetComponent<Animator>().SetTrigger("Chop");
        }

        _audioSource.clip = _clips[0];
        _audioSource.Play();
    }

    private void HandleRockHit(RaycastHit2D _hit)
    {
        var parent = _hit.collider.transform.parent;

        parent.GetComponent<Shake>().ShakeStart();

        var rockManager = parent.GetComponent<ObjectManager>();

        if (rockManager._objectHealth <= 0)
        {
            Instantiate(_rockParticle.gameObject, _hit.collider.gameObject.transform.position, Quaternion.identity);
            Destroy(_rockParticle.gameObject);
        }
        else
        {
            Instantiate(_rockParticle.gameObject, _hit.collider.gameObject.transform.position, Quaternion.identity);
        }

        rockManager.ObjectTakeDamage(1);

        _audioSource.clip = _clips[1];
        _audioSource.Play();
    }
}
