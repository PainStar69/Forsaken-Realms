using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [Header("Float's")]
    public float _intensity = 0.1f;
    public float _duration = 0.5f;
    private float _currentShakeTime = 0f;

    [Header("Vector's")]
    private Vector3 _originalPos;

    [Header("Bool's")]
    private bool _isShaking = false;

    void Start()
    {
        _originalPos = transform.localPosition;
    }

    public void ShakeStart()
    {
        _currentShakeTime = _duration;

        if (!_isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        _isShaking = true;

        while (_currentShakeTime > 0f)
        {
            float _offsetX = Random.Range(-_intensity, _intensity);
            float _offsetY = Random.Range(-_intensity, _intensity);

            transform.localPosition = _originalPos + new Vector3(_offsetX, _offsetY, 0f);

            _currentShakeTime -= Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originalPos;
        _isShaking = false;
    }
}
