using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [Header("GameObject's")]
    public GameObject[] _dropItems;
    public GameObject _breakObject;
    public GameObject _outline;

    [Header("Int's")]
    public int _objectHealth = 3;
    [SerializeField] int _dropCount;

    [Header("Float's")]
    [SerializeField] private float _spread;
    [SerializeField] private float _destroyTime;

    [Header("Bool's")]
    [SerializeField] private bool _animationActive;
    private bool _animActive;

    void Update()
    {
        if(_objectHealth <= 0)
        {
            if (!_animationActive)
            {
                Destroy(_outline);
                DropItem();
            }
            else
            {
                Destroy(_outline);
                _animActive = true;
            }
        }
    }

    void DropItem()
    {
        while(_dropCount > 0)
        {
            _dropCount -= 1;
            Vector3 _pos = _breakObject.transform.position;

            _pos.x += _spread * UnityEngine.Random.value - _spread / 2;
            _pos.y += _spread * UnityEngine.Random.value - _spread / 2;

            GameObject _gameObject = Instantiate(_dropItems[0]);
            _gameObject.transform.position = _pos;
        }

        Destroy(_breakObject, _destroyTime);
    }

    public void AnimationDropItem()
    {
        if (_animActive == true)
        {
            while (_dropCount > 0)
            {
                _dropCount -= 1;
                Vector3 _pos = _breakObject.transform.position;

                _pos.x += _spread * UnityEngine.Random.value - _spread / 2;
                _pos.y += _spread * UnityEngine.Random.value - _spread / 2;

                GameObject _gameObject = Instantiate(_dropItems[0]);
                _gameObject.transform.position = _pos;
            }

            Destroy(_breakObject);
        }
    }

    public void ObjectTakeDamage(int _damage)
    {
        _objectHealth -= _damage;
    }
}
