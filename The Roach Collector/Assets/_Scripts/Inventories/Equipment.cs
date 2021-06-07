using System;
using System.Collections.Generic;
using UnityEngine;
using Harris.Saving;
using Harris.UI.Inventories;
using Harris.Inventories;
using UnityEngine.UI;
using TGP.Control;

namespace Harris.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        Dictionary<EquipLocation, EquipableItem> _equippedItems = new Dictionary<EquipLocation, EquipableItem>();

        EquipLocation _currentlySelectedLocation = EquipLocation.None;

        public event Action EquipmentUpdated;

        private void Awake()
        {
            EquipmentUpdated += UpdateArmor;
        }

        private void UpdateArmor()
        {
            //TODO: Better way of doing this as this couples the player to the equipment
            PlayerController player = GetComponent<PlayerController>();
            //Reset the stats of the player
            player.ResetStats();

            foreach(EquipLocation location in GetAllPopulatedSlots())
            {
                ArmorConfig armor = GetItemInSlot(location) as ArmorConfig;
                if (armor != null)
                {
                    foreach(StatValues stat in armor.GetStatValues())
                    {
                        player.EquipStat(stat);
                    }
                }
            }
        }

        //Unequips an item
        public void Unequip()
        {
            if (_currentlySelectedLocation == EquipLocation.None) return;

            //Gets the item in the slot
            EquipableItem item = GetItemInSlot(_currentlySelectedLocation);

            //Removes the item
            RemoveItem(_currentlySelectedLocation);

            //Adds to first empty slot of the inventory
            GetComponent<Inventory>().AddToFirstEmptySlot(item, 1);

            //Closes the tooltip
            FindObjectOfType<ItemTooltip>().Close();

            //Sets the seleected location to none
            _currentlySelectedLocation = EquipLocation.None;
        }

        public void Select(EquipLocation location)
        {
            _currentlySelectedLocation = location;
        }

        public void DropSelected()
        {
            if (_currentlySelectedLocation == EquipLocation.None) return;

            //Gets the item
            EquipableItem item = GetItemInSlot(_currentlySelectedLocation);

            //Drops the item
            GameObject.FindGameObjectWithTag("Player").GetComponent<ItemDropper>().DropItem(item, 1);

            //Closes the tooltip
            ItemTooltip tooltip = GameObject.FindObjectOfType<ItemTooltip>();
            if (tooltip)
            {
               tooltip.Close();
            }

            //Removes the item
            RemoveItem(_currentlySelectedLocation);

            //Sets the selected to none
            _currentlySelectedLocation = EquipLocation.None;
        }



        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            //Check if we have the EquipLocation
            if (!_equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return _equippedItems[equipLocation];
        }

        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            //Add the item to a slot
            Debug.Assert(item.GetAllowedEquipLocation() == slot);

            _equippedItems[slot] = item;

            EquipmentUpdated?.Invoke();
        }

        public int GetIndexOfType(EquipLocation location)
        {

            int index = -1;
            //Find the index of the EquipLocation passed in
            for (int i = 0; i < _equippedItems.Count; i++) 
            {
                if (_equippedItems[location]) index = 1;
            }

            return index;
        }

        public void RemoveItem(EquipLocation slot)
        {
            //Remove the item from this slot
            _equippedItems.Remove(slot);
            EquipmentUpdated?.Invoke();
        }

        public void WipeEquipment()
        {
            _equippedItems = new Dictionary<EquipLocation, EquipableItem>();
            EquipmentUpdated?.Invoke();
        }

        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            //Returns the populated slots
            return _equippedItems.Keys;
        }


        //ISavable Interface Implementation
        object ISaveable.Save()
        {
            //Makes a dictionary of items we have equipped
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (var pair in _equippedItems)
            {
                //Sets the values to there item id
                equippedItemsForSerialization[pair.Key] = pair.Value.ItemID;
            }
            return equippedItemsForSerialization;
        }
        void ISaveable.Load(object state)
        {
            _equippedItems = new Dictionary<EquipLocation, EquipableItem>();

            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                //Equips the item based on its ID (pair.Value)
                var item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
                if (item != null)
                {
                    _equippedItems[pair.Key] = item;
                }
            }
        }
    }
}