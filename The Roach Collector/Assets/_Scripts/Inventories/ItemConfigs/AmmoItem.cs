using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("InventorySystem/Ammo Item"))]
public class AmmoItem : InventoryItem
{
    [Header("Ammo Settings")]
    [SerializeField] AmmoID _ammoID;
    public AmmoID AmmoType { get { return _ammoID; } }
}
