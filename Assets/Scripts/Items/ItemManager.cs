using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPos;
    private float fallTimer;
    private bool landed = false;

    [Header("Drop Settings")]
    public float dropForce = 3f;        // ilk f�rlatma g�c�
    public float dropDuration = 0.6f;   // yere s�z�lme s�resi
    public float bounceHeight = 0.5f;   // ne kadar yukar�dan d��ecek
    public float floatAmplitude = 0.05f; // yerde hafif dalgalanma
    public float floatSpeed = 6f;

    private Vector3 targetPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Rastgele bir y�ne do�ru hafif f�rlat
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        rb.AddForce(randomDir * dropForce, ForceMode2D.Impulse);

        // �Havadan d���yormu�� efekti i�in pozisyon ayarla
        startPos = transform.position + Vector3.up * bounceHeight;
        targetPos = transform.position;
        transform.position = startPos;
    }

    void Update()
    {
        if (!landed)
        {
            fallTimer += Time.deltaTime / dropDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, fallTimer);

            // d��me tamamlan�nca "yere indi" say
            if (fallTimer >= 1f)
            {
                landed = true;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                rb.drag = 3;
            }
        }
        else
        {
            // hafif "nefes alma" gibi minik z�plama efekti
            transform.localPosition += Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatAmplitude * Time.deltaTime;
        }
    }
}
