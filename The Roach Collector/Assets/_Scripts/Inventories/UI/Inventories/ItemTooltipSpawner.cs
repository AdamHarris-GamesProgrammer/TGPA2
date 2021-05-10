using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Core.UI.Tooltips;

namespace Harris.UI.Inventories
{
    [RequireComponent(typeof(IItemHolder))]
    public class ItemTooltipSpawner : TooltipSpawner
    {
        //Can we create a tooltip
        public override bool CanCreateTooltip()
        {
            //Gets the item in this slot
            var item = GetComponent<IItemHolder>().GetItem();

            //if we have not got an item then we can't create a tooltip
            if (!item) return false;

            //we can create a tooltip
            return true;
        }

        //Update our tooltip
        public override void UpdateTooltip(GameObject tooltip)
        {
            //Get the tooltip component
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();
            //if we do not have a tooltip then don't update the tooltip 
            if (!itemTooltip) return;

            //gets the item in this slot
            var item = GetComponent<IItemHolder>().GetItem();

            //Sets up the tooltip with the items name and description.
            itemTooltip.Setup(item);
        }
    }
}