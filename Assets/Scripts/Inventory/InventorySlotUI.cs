using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject _dropped = eventData.pointerDrag;
        if (_dropped == null) return;

        DraggableItem _draggableItem = _dropped.GetComponent<DraggableItem>();
        if (_draggableItem == null) return;

        if (transform.childCount == 0)
        {
            _draggableItem._parentAfterDrag = transform;
        }
        else
        {
            Transform _existingItem = transform.GetChild(0);
            Transform _originalParent = _draggableItem._parentAfterDrag;

            _draggableItem._parentAfterDrag = transform;

            _existingItem.SetParent(_originalParent);
            _existingItem.localPosition = Vector3.zero;
        }
    }
}
