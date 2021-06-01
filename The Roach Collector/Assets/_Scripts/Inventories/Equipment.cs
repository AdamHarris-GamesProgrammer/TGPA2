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


        // PUBLIC
        public void Unequip()
        {
            if (_currentlySelectedLocation == EquipLocation.None) return;

            EquipableItem item = GetItemInSlot(_currentlySelectedLocation);

            RemoveItem(_currentlySelectedLocation);

            GetComponent<Inventory>().AddToFirstEmptySlot(item, 1);

            FindObjectOfType<ItemTooltip>().Close();

            _currentlySelectedLocation = EquipLocation.None;
        }

        public void Select(EquipLocation location)
        {
            _currentlySelectedLocation = location;
            Debug.Log("Current selected location  is: " + _currentlySelectedLocation);
        }

        public void DropSelected()
        {
            if (_currentlySelectedLocation == EquipLocation.None) return;

            EquipableItem item = GetItemInSlot(_currentlySelectedLocation);

            GameObject.FindGameObjectWithTag("Player").GetComponent<ItemDropper>().DropItem(item, 1);

            ItemTooltip tooltip = GameObject.FindObjectOfType<ItemTooltip>();
            if (tooltip)
            {
               tooltip.Close();
            }

            RemoveItem(_currentlySelectedLocation);

            _currentlySelectedLocation = EquipLocation.None;
        }



        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!_equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return _equippedItems[equipLocation];
        }

        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            Debug.Assert(item.GetAllowedEquipLocation() == slot);

            _equippedItems[slot] = item;

            EquipmentUpdated?.Invoke();
        }

        public int GetIndexOfType(EquipLocation location)
        {
            int index = -1;

            for (int i = 0; i < _equippedItems.Count; i++) 
            {
                if (_equippedItems[location]) index = 1;
            }

            return index;
        }

        public void RemoveItem(EquipLocation slot)
        {
            _equippedItems.Remove(slot);
            EquipmentUpdated?.Invoke();
        }

        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            return _equippedItems.Keys;
        }


        //ISavable Interface Implementation
        object ISaveable.Save()
        {
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (var pair in _equippedItems)
            {
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
                var item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
                if (item != null)
                {
                    _equippedItems[pair.Key] = item;
                }
            }
        }
    }
}