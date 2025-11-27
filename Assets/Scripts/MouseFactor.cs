using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFactor : MonoBehaviour
{
    public string _targetLayerName;
    private int _layerMask;

    private GameObject _lastActiveChild;

    public Transform _player;
    public float _maxDistance = 1.2f;

    void Start()
    {
        _layerMask = LayerMask.GetMask(_targetLayerName);
    }

    void Update()
    {
        if (_lastActiveChild == null)
            _lastActiveChild = null;

        Vector2 _mousePosScreen = Mouse.current.position.ReadValue();
        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(_mousePosScreen);

        RaycastHit2D _hit = Physics2D.Raycast(_mousePos, Vector2.zero, 0f, _layerMask);

        if (_hit.collider != null)
        {
            float dist = Vector2.Distance(_player.position, _hit.collider.transform.position);
            if (dist > _maxDistance)
            {
                if (_lastActiveChild != null)
                {
                    _lastActiveChild.SetActive(false);
                    _lastActiveChild = null;
                }
                return;
            }

            Transform _child = _hit.collider.transform.childCount > 0
                ? _hit.collider.transform.GetChild(0)
                : null;

            if (_child != null)
            {
                if (_lastActiveChild != null && _lastActiveChild != _child.gameObject)
                {
                    if (_lastActiveChild != null)
                        _lastActiveChild.SetActive(false);
                }

                if (_child.gameObject != _lastActiveChild)
                {
                    _child.gameObject.SetActive(true);
                    _lastActiveChild = _child.gameObject;
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Debug.Log("Objeye týklandý: " + _hit.collider.name);
                }

                return;
            }
        }

        if (_lastActiveChild != null)
        {
            _lastActiveChild.SetActive(false);
            _lastActiveChild = null;
        }
    }
}
