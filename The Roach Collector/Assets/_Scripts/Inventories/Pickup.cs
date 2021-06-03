using UnityEngine;

namespace Harris.Inventories
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] InventoryItem _item;
        [SerializeField] int _number = 1;

        Inventory _inventory;

        

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
           
            _inventory = player.GetComponent<Inventory>();
        }

        public void Setup(InventoryItem item, int number)
        {
            //Sets up the basic information for a pickup
            _item = item;
            if (!item.IsStackable) number = 1;
            _number = number;
        }

        public InventoryItem GetItem()
        {
            return _item;
        }

        public int GetNumber()
        {
            return _number;
        }

        public void PickupItem()
        {
            //Adds the item to the first empty slot
            bool foundSlot = _inventory.AddToFirstEmptySlot(_item, _number);
            //if we can pickup an item destroy this pickup object
            if (foundSlot)
            {
                PickupAudio pickupAudio = GameObject.FindGameObjectWithTag("PickupSound").GetComponent<PickupAudio>();
                pickupAudio.PlayPickupSound();
                
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()
        {
            return _inventory.HasSpaceFor(_item);
        }
    }
}