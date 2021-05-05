using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    private Inventory _inventory;
    public Inventory ChestInventory { get { return _inventory; } set { _inventory = value; } }


}
