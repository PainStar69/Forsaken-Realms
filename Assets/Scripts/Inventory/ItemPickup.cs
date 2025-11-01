using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData _itemData;
    public int _amount = 1;

    [System.Obsolete]
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Inventory.Instance.AddItem(_itemData, _amount))
            {
                Destroy(gameObject);
            }
        }
    }
}
