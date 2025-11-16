using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFactor : MonoBehaviour
{
    public string targetLayerName;
    private int layerMask;

    private GameObject lastActiveChild;

    void Start()
    {
        layerMask = LayerMask.GetMask(targetLayerName);
    }

    void Update()
    {
        Vector2 mousePosScreen = Mouse.current.position.ReadValue();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mousePosScreen);

        // Sadece target layer'a raycast atýlýyor
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, layerMask);

        if (hit.collider != null)
        {
            Transform child = hit.collider.transform.childCount > 0
                ? hit.collider.transform.GetChild(0)
                : null;

            if (child != null)
            {
                if (child.gameObject != lastActiveChild)
                {
                    lastActiveChild?.SetActive(false);
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
