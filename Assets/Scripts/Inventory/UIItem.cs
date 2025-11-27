using UnityEngine;
using TMPro;

public class UIItem : MonoBehaviour
{
    public InventorySlot boundSlot;  // Bu UI'nýn temsil ettiði gerçek envanter slotu
    public TMP_Text amountText;      // Prefab içinde amount text

    void Update()
    {
        if (boundSlot == null) return;

        int amount = boundSlot._amount;

        amountText.text = amount > 1 ? amount.ToString() : "";
    }
}