using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFactor : MonoBehaviour
{
    public string targetLayerName;
    private int layerMask;

    private GameObject lastActiveChild;

    public Transform player;   // Mesafe için oyuncu transformu
    public float maxDistance = 1.2f;

    void Start()
    {
        layerMask = LayerMask.GetMask(targetLayerName);
    }

    void Update()
    {
        // Destroy edilen objeyi temizle
        if (lastActiveChild == null)
            lastActiveChild = null;

        Vector2 mousePosScreen = Mouse.current.position.ReadValue();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mousePosScreen);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, layerMask);

        if (hit.collider != null)
        {
            // --- MESAFE KONTROLÜ ---
            float dist = Vector2.Distance(player.position, hit.collider.transform.position);
            if (dist > maxDistance)
            {
                // Uzaksa outline'ý kapat
                if (lastActiveChild != null)
                {
                    lastActiveChild.SetActive(false);
                    lastActiveChild = null;
                }
                return;
            }
            // -----------------------

            Transform child = hit.collider.transform.childCount > 0
                ? hit.collider.transform.GetChild(0)
                : null;

            if (child != null)
            {
                if (lastActiveChild != null && lastActiveChild != child.gameObject)
                {
                    if (lastActiveChild != null)
                        lastActiveChild.SetActive(false);
                }

                if (child.gameObject != lastActiveChild)
                {
                    child.gameObject.SetActive(true);
                    lastActiveChild = child.gameObject;
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Debug.Log("Objeye týklandý: " + hit.collider.name);
                }

                return;
            }
        }

        if (lastActiveChild != null)
        {
            lastActiveChild.SetActive(false);
            lastActiveChild = null;
        }
    }
}
