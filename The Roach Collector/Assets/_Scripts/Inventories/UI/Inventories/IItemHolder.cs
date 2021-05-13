using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Inventories;

namespace Harris.UI.Inventories
{
    public interface IItemHolder
    {
        //Gets the item from the slot
        InventoryItem GetItem();
    }
}