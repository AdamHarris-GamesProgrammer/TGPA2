using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Harris.Saving;

namespace Harris.Inventories
{
    public class ItemDropper : MonoBehaviour
    {
        public void DropItem(InventoryItem item, int number)
        {
            //spawns a pickup at the desired location
            SpawnPickup(item, GetDropLocation(), number);
        }

        public void DropItem(InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation(), 1);
        }

        protected virtual Vector3 GetDropLocation()
        {
            return transform.position;
        }


        public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            //Creates the pickup for the item at the spawn location provided
            var pickup = item.SpawnPickup(spawnLocation, number);
        }
    }
}