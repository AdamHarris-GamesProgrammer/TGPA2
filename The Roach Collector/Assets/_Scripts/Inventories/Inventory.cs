using System;
using UnityEngine;
using Harris.Saving;
using Harris.UI.Inventories;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Harris.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        [Tooltip("Allowed size")]
        [SerializeField] int _inventorySize = 16;


        InventorySlot[] _slots;
        int _currentSelectedSlot;

        private void Awake()
        {
            _slots = new InventorySlot[_inventorySize];
            _currentSelectedSlot = -1;
        }

        // Broadcasts when the items in the slots are added/removed.
        public event Action InventoryUpdated;

        /// Struct that represents everything to do with a Inventory Slot. Contains the item and number of that item
        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }

        public InventorySlot GetInventorySlot(int index) {
            return  _slots[index];
        }


        public InventorySlot[] GetFilledSlots()
        {
            //Stores slots which have a item in them
            List<InventorySlot> filledSlots = new List<InventorySlot>();

            //Cycle through each inventory slot
            foreach(InventorySlot slot in _slots)
            {
                //does the slot contain an item
                if(slot.item  != null)
                {
                    //Add the slot to the list if there is a item
                    filledSlots.Add(slot);
                }
            }

            //Converts the list to a array and returns it
            return filledSlots.ToArray();
        }


        public void SelectItem(int index)
        {
            _currentSelectedSlot = index;
        }

        public void EquipItem()
        {
            //When currentSelectedSlot is -1 it means no slot is selected
            if (_currentSelectedSlot == -1) return;

            EquipableItem item = _slots[_currentSelectedSlot].item as EquipableItem;

            if (!item) return;

            Equipment equipment = GetComponent<Equipment>();

            int result = equipment.GetIndexOfType(item.GetAllowedEquipLocation());

            //Get the currently equipped item if it exists
            InventoryItem newItem = null;
            if (result > 0)
            {
                newItem = equipment.GetItemInSlot(item.GetAllowedEquipLocation());
            }

            equipment.AddItem(item.GetAllowedEquipLocation(), item);
            RemoveFromSlot(_currentSelectedSlot, 1);

            //Equip the old equipped item if there was a item equipped
            if (newItem != null) AddToFirstEmptySlot(newItem, 1);

            InventorySlotUI[] ui;

            //Change currentSelectedSlot back to non-selected
            _currentSelectedSlot = -1;
        }

        public void DropSelected()
        {
            //safety check that a object is selected
            if (_currentSelectedSlot == -1) return;

            //Gets the item to drop
            InventoryItem item = GetItemInSlot(_currentSelectedSlot);

            //Gets the item dropper and drops the item
            GameObject.FindGameObjectWithTag("Player").GetComponent<ItemDropper>().DropItem(item);

            //TODO: Add drop dialog for when player has more than 3 items of a type

            //Removes the item from the currently selected slot
            RemoveFromSlot(_currentSelectedSlot, 1);

            
            InventorySlotUI[] ui;

            //Finds the SlotUIs
            ui = FindObjectsOfType<InventorySlotUI>();


            //Sets the current slot to -1
            _currentSelectedSlot = -1;
        }

        public static Inventory GetPlayerInventory()
        {
            //Gets the inventory from the player
            var player = GameObject.FindWithTag("Player");
            if (!player) return null;
            return player.GetComponent<Inventory>();
        }

        public bool HasSpaceFor(InventoryItem item)
        {
            //Checks if we have space for a item
            return FindSlot(item) >= 0;
        }

        public int GetSize()
        {
            //Gets the size of our inventory
            return _slots.Length;
        }

        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            //finds a slot for our item
            int i = FindSlot(item);

            //We dont have a slot
            if (i < 0)
            {
                return false;
            }

            //Sets the slot to the item
            _slots[i].item = item;
            _slots[i].number += number;

            //Updates the inventory
            if (InventoryUpdated != null)
            {
                InventoryUpdated();
            }
            return true;
        }

        public bool HasItem(InventoryItem item)
        {
            //Cycles through our inventory and sees if we have an item
            for (int i = 0; i < _slots.Length; i++)
            {
                if (object.ReferenceEquals(_slots[i].item, item)) return true;
            }
            return false;
        }

        public InventoryItem GetItemInSlot(int slot)
        {
            //Gets an item from a specified index
            return _slots[slot].item;
        }

        public int GetNumberInSlot(int slot)
        {
            //Gets the number of items in the slot specified
            return _slots[slot].number;
        }

        public void RemoveFromSlot(int slot, int number)
        {
            //Removes X amount of items from the slot
            _slots[slot].number -= number;
            //Removes the item if the number is now less than 0
            if (_slots[slot].number <= 0)
            {
                _slots[slot].number = 0;
                _slots[slot].item = null;
            }
            //Update the inventory
            if (InventoryUpdated != null)
            {
                InventoryUpdated();
            }
        }

        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            //if the slot has an item in it 
            if (_slots[slot].item != null)
            {
                //Add to first empty slot instead (handles logic for items which already belong together)
                return AddToFirstEmptySlot(item, number); ;
            }

            //Finds the index for the item
            var i = FindStack(item);

            //if we found a slot
            if (i >= 0)
            {
                slot = i;
            }

            //Set the slot to this item
            _slots[slot].item = item;
            _slots[slot].number += number;
            if (InventoryUpdated != null)
            {
                InventoryUpdated();
            }
            return true;
        }

        public void WipeInventory()
        {
            Debug.Log("Wipe");
            //Removes all items from the inventory
            int index = 0;
            foreach(InventorySlot slot in _slots)
            {
                RemoveFromSlot(index, slot.number);

                index++;
            }
            
        }


        public int FindItem(InventoryItem item)
        {
            //Finds the index of an item
            int i = FindStack(item);
            if (i < 0)
            {
                //there isnt a stack of this item so return a empty slot
                i = FindEmptySlot();
            }
            return i;
        }

        public int Find(InventoryItem item) {
            //Find the specified item in the inventory
            for (int i = 0; i < _slots.Length; i++)
            {
                //Check if the references match for the current item and the passed in item
                if (object.ReferenceEquals(_slots[i].item, item)) return i;
            }
            return -1;
        }
        //Find a slot for the item 
        private int FindSlot(InventoryItem item)
        {
            //Find the index of the stack
            int i = FindStack(item);
            if (i < 0)
            {
                //find an empty slot
                i = FindEmptySlot();
            }
            //return the index
            return i;
        }

        //Finds an empty slot
        private int FindEmptySlot()
        {
            //Cycles through the inventory and checks for a null slot
            for (int i = 0; i < _slots.Length; i++)
            {
                //If the item is null then return this index
                if (_slots[i].item == null) return i;
            }
            return -1;
        }

        private int FindStack(InventoryItem item)
        {
            //if the item is not stackable
            if (!item.IsStackable) return -1;

            //Cycle through the inventory
            for (int i = 0; i < _slots.Length; i++)
            {
                //if the references match, return i
                if (object.ReferenceEquals(_slots[i].item, item)) return i;
            }
            return -1;
        }


        [System.Serializable]
        private struct InventorySlotRecord
        {
            //Stores the id of the item
            public string itemID;
            //Stores the amount of that item we have 
            public int number;
        }
    
        object ISaveable.Save()
        {
            var slotStrings = new InventorySlotRecord[_inventorySize];
            for (int i = 0; i < _inventorySize; i++)
            {
                if (_slots[i].item != null)
                {
                    Debug.Log("Saving " + _slots[i].item.Name);
                    slotStrings[i].itemID = _slots[i].item.ItemID;
                    slotStrings[i].number = _slots[i].number;
                }
            }
            return slotStrings;
        }

        void ISaveable.Load(object state)
        {
            _slots = new InventorySlot[_inventorySize];
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < _inventorySize; i++)
            {
                if (slotStrings[i].itemID == null) continue;

                _slots[i].item = InventoryItem.GetFromID(slotStrings[i].itemID);

                _slots[i].number = slotStrings[i].number;
            }
            if (InventoryUpdated != null)
            {
                InventoryUpdated();
            }
        }
    }
}