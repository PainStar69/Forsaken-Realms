using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [Header("GameObject's")]
    public GameObject[] _dropItems;

    [Header("Int's")]
    [SerializeField] int _objectHealth = 3;
    [SerializeField] int _dropCount;

    [Header("Float's")]
    [SerializeField] private float _spread;

    void Update()
    {
        if(_objectHealth <= 0)
        {
            DropItem();
        }
    }

    void DropItem()
    {
        while(_dropCount > 0)
        {
            _dropCount -= 1;
            Vector3 _pos = transform.position;

            _pos.x += _spread * UnityEngine.Random.value - _spread / 2;
            _pos.y += _spread * UnityEngine.Random.value - _spread / 2;

            GameObject _gameObject = Instantiate(_dropItems[0]);
            _gameObject.transform.position = _pos;
        }

        Destroy(gameObject);
    }

    public void ObjectTakeDamage(int _damage)
    {
        _objectHealth -= _damage;
    }
}
