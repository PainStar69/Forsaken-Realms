using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float intensity = 0.1f;   // Titreme b�y�kl���
    public float duration = 0.5f;    // Titreme s�resi

    private Vector3 originalPos;
    private float currentShakeTime = 0f;
    private bool isShaking = false;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void ShakeStart()
    {
        // Titreme s�resini resetle
        currentShakeTime = duration;

        // E�er zaten titremiyorsa, ba�lat
        if (!isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;

        while (currentShakeTime > 0f)
        {
            // Her frame yeni pozisyon �ret
            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            // S�reyi azalt
            currentShakeTime -= Time.deltaTime;
            yield return null;
        }

        // Bitince pozisyonu s�f�rla
        transform.localPosition = originalPos;
        isShaking = false;
    }
}
