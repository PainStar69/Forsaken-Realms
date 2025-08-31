using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnesBossController : MonoBehaviour
{
    [Header("Prefab's")]
    [SerializeField] private GameObject _magicCircle;

    [Header("Int's")]
    [SerializeField] private int _attackCount;

    [Header("GameObject's")]
    private GameObject _character;

    void Awake()
    {
        _character = GameObject.Find("Player");
    }


    void Update()
    {
        if(_attackCount == 0)
        {
            GameObject _magic =Instantiate(_magicCircle, _character.transform.position, Quaternion.identity);
            _attackCount = 1;
        }
    }
}
