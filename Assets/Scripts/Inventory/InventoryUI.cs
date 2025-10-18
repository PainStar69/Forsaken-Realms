using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Image> _slotImages;
    public List<TMP_Text> _amountTexts; //(Show Item Stack Count)

    public void RefreshUI(List<InventorySlot> _items)
    {
        for (int i = 0; i < _slotImages.Count; i++)
        {
            if (i < _items.Count)
            {
                _slotImages[i].sprite = _items[i]._item._icon;
                _slotImages[i].color = Color.white;

                if (_amountTexts != null && _amountTexts.Count > i)
                    _amountTexts[i].text = _items[i]._amount > 1 ? _items[i]._amount.ToString() : "";
            }
            else
            {
                _slotImages[i].sprite = null;
                _slotImages[i].color = new Color(1, 1, 1, 0); // Boþ slot görünmesin
                if (_amountTexts != null && _amountTexts.Count > i)
                    _amountTexts[i].text = "";
            }
        }
    }
}
