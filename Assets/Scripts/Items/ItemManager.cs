using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Float Settings")]
    public float _floatAmplitude = 0.05f;
    public float _floatSpeed = 6f;

    void Update()
    {
        transform.localPosition += Vector3.up * Mathf.Sin(Time.time * _floatSpeed) * _floatAmplitude * Time.deltaTime;
    }
}
