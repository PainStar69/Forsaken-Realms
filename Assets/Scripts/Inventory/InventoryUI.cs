using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Transform> _slotParents;
    public GameObject _itemPrefab;

    private Dictionary<ItemData, GameObject> _spawnedItems = new Dictionary<ItemData, GameObject>();

    public void RefreshUI(List<InventorySlot> _items)
    {
        foreach (var slot in _items)
        {
            if (slot._item == null) continue;

            // UI zaten var ise atla (UIItem kendi amountunu güncelleyecek zaten)
            if (_spawnedItems.ContainsKey(slot._item))
                continue;

            // Boþ slot al
            Transform emptySlot = GetFirstEmptySlot();
            if (emptySlot == null) continue;

            // Yeni item UI oluþtur
            GameObject itemUI = Instantiate(_itemPrefab, emptySlot);

            Image img = itemUI.GetComponent<Image>();
            img.sprite = slot._item._icon;
            img.color = Color.white;

            itemUI.transform.localPosition = Vector3.zero;
            itemUI.transform.localScale = Vector3.one;
            itemUI.name = slot._item.name;

            // Drag için parent
            DraggableItem drag = itemUI.GetComponent<DraggableItem>();
            drag._parentAfterDrag = emptySlot;

            // UIItem'a slot referansý ver
            UIItem uiItem = itemUI.GetComponent<UIItem>();
            uiItem.boundSlot = slot;

            // Dictionary'ye ekle
            _spawnedItems.Add(slot._item, itemUI);
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
}
