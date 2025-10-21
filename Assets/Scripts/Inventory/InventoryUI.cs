using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public List<Transform> _slotParents;
    public GameObject _itemPrefab;
    public List<TMP_Text> _amountTexts;

    public void RefreshUI(List<InventorySlot> _items)
    {
        foreach (var slot in _slotParents)
        {
            foreach (Transform child in slot)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < _slotParents.Count; i++)
        {
            if (i < _items.Count)
            {
                GameObject itemUI = Instantiate(_itemPrefab, _slotParents[i]);
                Image img = itemUI.GetComponent<Image>();
                itemUI.name = _items[i]._item.name;
                img.sprite = _items[i]._item._icon;
                img.color = Color.white;
                itemUI.transform.localPosition = Vector3.zero;
                itemUI.transform.localScale = Vector3.one;

                if (_amountTexts != null && _amountTexts.Count > i)
                    _amountTexts[i].text = _items[i]._amount > 1 ? _items[i]._amount.ToString() : "";
            }
            else
            {
                if (_amountTexts != null && _amountTexts.Count > i)
                    _amountTexts[i].text = "";
            }
        }
    }
}
