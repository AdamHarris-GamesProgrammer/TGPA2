using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Inventories;
public class SellableItem : InventoryItem
{
    [Header("Sellable Settings")]
    [SerializeField] float _itemValue = 25.0f;

    public float ItemValue {get {return _itemValue; }}

}
