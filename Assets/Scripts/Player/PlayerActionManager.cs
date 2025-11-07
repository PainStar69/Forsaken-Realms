using System.Collections;
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

    [Header("Camera's")]
    public Camera _mainCam;

    [Header("Script's")]
    public InventoryUI _inventoryUI;

    [Header("MP3")]
    public AudioClip[] _clips;

    [Header("Gameobject's")]
    public ParticleSystem _rockParticle;

    [Header("Bool's")]
    public bool _axe;
    public bool _pickaxe;
    public bool _sword;

    [Header("Float's")]
    public float _detectDistance = 1f;
    [SerializeField] private float _dist;

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

        _mainCam = Camera.main;
    }

    #region InputSystem

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
        else if (_sword)
        {
            _anim.SetTrigger("Sword");
            PlayerMovement.moveSpeed = 0;
        }
    }

    private int _lastSelectedSlotIndex = -1;
    private int _slotIndex;

    private void OnAlphaButtonClick(InputAction.CallbackContext _ctx)
    {
        var _control = _ctx.control;

        if (!int.TryParse(_control.name, out int num)) return;
        _slotIndex = num - 1;

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

        // Slot içeriğini kontrol et
        if (slotTransform.childCount > 0)
        {
            Transform _item = slotTransform.GetChild(0);
            string _itemName = _item.name;

            if (_itemName.Contains("Axe"))
            {
                _axe = true;
                _pickaxe = false;
                _sword = false;
            }
            else if (_itemName.Contains("Pickaxe"))
            {
                _pickaxe = true;
                _axe = false;
                _sword = false;
            }
            else if (_itemName.Contains("Sword"))
            {
                _sword = true;
                _axe = false;
                _pickaxe = false;
            }
            else
            {
                _axe = false;
                _pickaxe = false;
                _sword = false;
            }
        }
        else
        {
            _axe = false;
            _pickaxe = false;
            _sword = false;
        }

        // Son seçili olarak kaydet
        _lastSelectedSlotIndex = _slotIndex;
    }

    public void RecheckSlots()
    {
        Transform slotTransform = _inventoryUI._slotParents[_slotIndex];

        if (slotTransform.childCount > 0)
        {
            Transform _item = slotTransform.GetChild(0);
            string _itemName = _item.name;

            Debug.Log(_itemName);

            if (_itemName.Contains("Axe"))
            {
                _axe = true;
                _pickaxe = false;
                _sword = false;
            }
            else if (_itemName.Contains("Pickaxe"))
            {
                _pickaxe = true;
                _axe = false;
                _sword = false;
            }
            else if(_itemName.Contains("Sword"))
            {
                _sword = true;
                _axe = false;
                _pickaxe = false;
            }
            else
            {
                _axe = false;
                _pickaxe = false;
                _sword = false;
            }
        }
        else
        {
            _axe = false;
            _pickaxe = false;
            _sword = false;
        }
    }

    #endregion

    // Object Detection
    public void DetectObjectInFront()
    {
        var _ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        var _hit = Physics2D.GetRayIntersection(_ray, Mathf.Infinity, _objectLayer);

        if (!_hit.collider) return;
        if(_hit.collider.gameObject.name == "AxeCollision" && _axe)
        {
            _dist = Vector2.Distance(_hit.collider.gameObject.transform.position, transform.position);

            if(_dist < 1.2f)
            HandleTreeHit(_hit);
        }
        else if(_hit.collider.gameObject.name == "PickaxeCollision" && _pickaxe)
        {
            _dist = Vector2.Distance(_hit.collider.gameObject.transform.position, transform.position);

            if (_dist < 1.2f)
            HandleRockHit(_hit);
        }
        else
        {
            Debug.Log(_hit.collider.gameObject.name);
        }
    }

    #region Private Regions

    private void HandleTreeHit(RaycastHit2D _hit)
    {
        var _parent = _hit.collider.transform.parent;

        ParticleSystem _treeParticle = _parent.Find("TreeParticle").GetComponent<ParticleSystem>();
        _parent.GetComponent<Shake>().ShakeStart();

        var treeChop = _parent.Find("TreeChop").GetComponent<ObjectManager>();
        Transform treeChopTransform = treeChop.transform;

        Transform _firstChild = treeChopTransform.GetChild(0);
        Transform _secondChild = treeChopTransform.GetChild(1);

        if (treeChop._objectHealth <= 1)
        {
            Destroy(_treeParticle.gameObject);
        }
        else
        {
            _treeParticle.Play();
        }

        treeChop.ObjectTakeDamage(1);

        if(_firstChild.GetComponent<ReverseTree>()._right == true)
        {
            gameObject.GetComponent<PlayerMovement>().moveInput.x = -1;
            StartCoroutine(Refreshed());
        }
        else if (_secondChild.GetComponent<ReverseTree>()._left == true)
        {
            gameObject.GetComponent<PlayerMovement>().moveInput.x = 1;
            StartCoroutine(Refreshed());
        }

        if (treeChop._objectHealth <= 0)
        {
            if(_firstChild.GetComponent<ReverseTree>()._right == true)
            {
                treeChop.GetComponent<Animator>().SetTrigger("ChopLeft");
            }
            else if(_secondChild.GetComponent<ReverseTree>()._left == true)
            {
                treeChop.GetComponent<Animator>().SetTrigger("ChopRight");
            }
            else
            {
                treeChop.GetComponent<Animator>().SetTrigger("ChopRight");
            }
        }

        _audioSource.clip = _clips[0];
        _audioSource.Play();
    }

    private void HandleRockHit(RaycastHit2D _hit)
    {
        var _parent = _hit.collider.transform.parent;

        _parent.GetComponent<Shake>().ShakeStart();

        var rockManager = _parent.GetComponent<ObjectManager>();

        var _firstChild = _hit.collider.transform.GetChild(0);
        var _secondChild = _hit.collider.transform.GetChild(1);

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

        if (_firstChild.GetComponent<ReverseTree>()._right == true)
        {
            gameObject.GetComponent<PlayerMovement>().moveInput.x = -1;
            StartCoroutine(Refreshed());
        }
        else if (_secondChild.GetComponent<ReverseTree>()._left == true)
        {
            gameObject.GetComponent<PlayerMovement>().moveInput.x = 1;
            StartCoroutine(Refreshed());
        }

        _audioSource.clip = _clips[1];
        _audioSource.Play();
    }

    IEnumerator Refreshed()
    {
        yield return new WaitForSeconds(.1f);
        gameObject.GetComponent<PlayerMovement>().moveInput.x = 0;
    }

    #endregion
}
