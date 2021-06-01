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
            int index = Random.Range(0, _items.Length);

            RandomPickupItem pickup = _items[index];

            int amount = Random.Range(pickup._minAmount, pickup._maxAmount);

            var spawnedPickup = pickup._item.SpawnPickup(transform.position, amount);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }
    }
}