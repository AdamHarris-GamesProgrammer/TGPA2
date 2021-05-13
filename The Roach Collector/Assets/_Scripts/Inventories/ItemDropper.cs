using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Harris.Saving;

namespace Harris.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        private List<Pickup> _droppedItems = new List<Pickup>();
        private List<DropRecord> _otherSceneDroppedItems = new List<DropRecord>();
        public void DropItem(InventoryItem item, int number)
        {
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
            var pickup = item.SpawnPickup(spawnLocation, number);
            _droppedItems.Add(pickup);
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
            public int sceneIndex;
        }

        object ISaveable.Save()
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            RemoveDestroyedDrops();
            var droppedItemsList = new List<DropRecord>();
            foreach(Pickup pickup in _droppedItems)
            {
                var droppedItem = new DropRecord();
                
                droppedItem.itemID = pickup.GetItem().ItemID;
                droppedItem.position = new SerializableVector3(pickup.transform.position);
                droppedItem.number = pickup.GetNumber();
                droppedItem.sceneIndex = sceneIndex;

                droppedItemsList.Add(droppedItem);
                
            }
            droppedItemsList.AddRange(_otherSceneDroppedItems);
            return droppedItemsList;
        }

        void ISaveable.Load(object state)
        {
            var droppedItemsList = (List<DropRecord>)state;
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            _otherSceneDroppedItems.Clear();

            foreach (var item in droppedItemsList)
            {
                if(item.sceneIndex != sceneIndex)
                {
                    _otherSceneDroppedItems.Add(item);
                    continue;
                }

                var pickupItem = InventoryItem.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector();
                int number = item.number;
                SpawnPickup(pickupItem, position, number);
            }
        }

        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in _droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            _droppedItems = newList;
        }
    }
}