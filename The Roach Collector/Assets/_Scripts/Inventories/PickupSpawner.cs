using UnityEngine;
using Harris.Saving;

namespace Harris.Inventories
{
    public class PickupSpawner : MonoBehaviour
    {
        [System.Serializable]
        public struct RandomPickupItem
        {
            public InventoryItem _item;
            public int _minAmount;
            public int _maxAmount;
        }

        // CONFIG DATA
        [SerializeField] RandomPickupItem[] _items;

        private void Awake()
        {
            SpawnPickup();
        }

        public Pickup GetPickup() 
        { 
            return GetComponentInChildren<Pickup>();
        }

        public bool isCollected() 
        { 
            return GetPickup() == null;
        }


        private void SpawnPickup()
        {
            //Generates a random index
            int index = Random.Range(0, _items.Length);

            //Gets that item
            RandomPickupItem pickup = _items[index];

            //Generates a random amount 
            int amount = Random.Range(pickup._minAmount, pickup._maxAmount);

            //Spawn X amount of that pickup
            var spawnedPickup = pickup._item.SpawnPickup(transform.position, amount);
            spawnedPickup.transform.SetParent(transform);
        }
    }
}