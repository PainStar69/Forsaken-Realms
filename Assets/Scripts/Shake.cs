using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float intensity = 0.1f;   // Titreme b�y�kl���
    public float duration = 0.5f;    // Titreme s�resi

    private Vector3 originalPos;
    private float shakeTime = 0f;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void ShakeStart()
    {
        shakeTime = duration; // Titremeyi ba�lat
    }

    void Update()
    {
        if (shakeTime > 0)
        {
            // Rastgele X ve Y ofset
            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            shakeTime -= Time.deltaTime;
        }
        else
        {
            // Titreme bitti�inde pozisyonu geri al
            transform.localPosition = originalPos;
        }
    }
}
