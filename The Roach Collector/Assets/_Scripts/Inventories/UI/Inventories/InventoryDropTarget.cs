using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Harris.Core.UI.Dragging;
using Harris.Inventories;

namespace Harris.UI.Inventories
{
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
    {
        public void AddItems(InventoryItem item, int number)
        {
            //Finds the player
            var player = GameObject.FindGameObjectWithTag("Player");

            //Drops items near the player
            player.GetComponent<ItemDropper>().DropItem(item, number);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return int.MaxValue;
        }
    }
}