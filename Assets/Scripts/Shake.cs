using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float intensity = 0.1f;   // Titreme büyüklüðü
    public float duration = 0.5f;    // Titreme süresi

    private Vector3 originalPos;
    private float currentShakeTime = 0f;
    private bool isShaking = false;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void ShakeStart()
    {
        // Titreme süresini resetle
        currentShakeTime = duration;

        // Eðer zaten titremiyorsa, baþlat
        if (!isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;

        while (currentShakeTime > 0f)
        {
            // Her frame yeni pozisyon üret
            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            // Süreyi azalt
            currentShakeTime -= Time.deltaTime;
            yield return null;
        }

        // Bitince pozisyonu sýfýrla
        transform.localPosition = originalPos;
        isShaking = false;
    }
}
