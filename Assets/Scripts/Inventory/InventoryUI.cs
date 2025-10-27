using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public List<Transform> _slotParents;
    public GameObject _itemPrefab;
    public List<TMP_Text> _amountTexts;

    private Dictionary<ItemData, GameObject> _spawnedItems = new Dictionary<ItemData, GameObject>();

    public void RefreshUI(List<InventorySlot> _items)
    {
        foreach (var slot in _items)
        {
            if (slot._item == null) continue;

            // E�er zaten UI'da varsa, sadece miktar�n� g�ncelle
            if (_spawnedItems.ContainsKey(slot._item))
            {
                UpdateAmountText(slot);
                continue;
            }

            // Bo� slot bul
            Transform emptySlot = GetFirstEmptySlot();
            if (emptySlot == null) continue;

            // Yeni item olu�tur
            GameObject itemUI = Instantiate(_itemPrefab, emptySlot);
            Image img = itemUI.GetComponent<Image>();
            itemUI.name = slot._item.name;
            img.sprite = slot._item._icon;
            img.color = Color.white;

            itemUI.transform.localPosition = Vector3.zero;
            itemUI.transform.localScale = Vector3.one;

            // DraggableItem i�indeki referanslar� ayarla
            DraggableItem draggable = itemUI.GetComponent<DraggableItem>();
            draggable._parentAfterDrag = emptySlot;

            // Dictionary'e kaydet
            _spawnedItems.Add(slot._item, itemUI);

            // Miktar� g�ncelle
            UpdateAmountText(slot);
        }
    }

    private Transform GetFirstEmptySlot()
    {
        foreach (Transform slot in _slotParents)
        {
            if (slot.childCount == 0)
                return slot;
        }
        return null;
    }

    private void UpdateAmountText(InventorySlot slot)
    {
        int index = _slotParents.FindIndex(s => s.childCount > 0 && s.GetChild(0).name == slot._item.name);
        if (index >= 0 && index < _amountTexts.Count)
        {
            _amountTexts[index].text = slot._amount > 1 ? slot._amount.ToString() : "";
        }
    }
}