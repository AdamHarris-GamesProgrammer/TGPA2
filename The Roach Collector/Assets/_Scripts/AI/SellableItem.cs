using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Inventories;

[CreateAssetMenu(menuName ="InventorySystem/Sellable Item")]
public class SellableItem : InventoryItem
{
    [Header("Sellable Settings")]
    //Holds how valuable the item is
    [SerializeField] float _itemValue = 25.0f;


    public float ItemValue {get {return _itemValue; }}

}
