using UnityEngine;
using Harris.Saving;

namespace Harris.Inventories
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        // CONFIG DATA
        [SerializeField] InventoryItem _item = null;
        [SerializeField] int _number = 1;

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
            var spawnedPickup = _item.SpawnPickup(transform.position, _number);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }

        object ISaveable.Save()
        {
            return isCollected();
        }

        void ISaveable.Load(object state)
        {
            bool shouldBeCollected = (bool)state;

            if (shouldBeCollected && !isCollected())
            {
                DestroyPickup();
            }

            if (!shouldBeCollected && isCollected())
            {
                SpawnPickup();
            }
        }
    }
}