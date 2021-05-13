using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Inventories;

namespace Harris.UI.Inventories
{
    /// <summary>
    /// Allows the `ItemTooltipSpawner` to display the right information.
    /// </summary>
    public interface IItemHolder
    {
        InventoryItem GetItem();
    }
}