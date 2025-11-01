using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData _item;
    public int _amount;

    public InventorySlot(ItemData _item, int _amount)
    {
        this._item = _item;
        this._amount = _amount;
    }
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; // Tekil eriþim için
    public int pocketSlots = 9;
    public int mainSlots = 9;

    public List<InventorySlot> _items = new List<InventorySlot>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    [System.Obsolete]
    public bool AddItem(ItemData _newItem, int _amount)
    {
        if (_newItem._isStackable)
        {
            InventorySlot existingSlot = _items.Find(x => x._item == _newItem);
            if (existingSlot != null)
            {
                existingSlot._amount += _amount;
                UpdateUI();
                return true;
            }
        }

        // Yeni slot aç
        if (_items.Count < pocketSlots + mainSlots)
        {
            _items.Add(new InventorySlot(_newItem, _amount));
            UpdateUI();
            return true;
        }

        Debug.Log("Envanter dolu!");
        return false;
    }

    [System.Obsolete]
    public void UpdateUI()
    {
        //Bütün ayný UI ve tekli UI'larý Scriptinden çaðýrýyoruz
        foreach (InventoryUI ui in FindObjectsOfType<InventoryUI>())
        {
            ui.RefreshUI(_items);
        }
    }
}
