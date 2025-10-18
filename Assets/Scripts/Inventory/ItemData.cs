using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items")]
public class ItemData : ScriptableObject
{
    public string _itemName;
    public Sprite _icon;
    public bool _isStackable;
    public int _maxStack;
}
