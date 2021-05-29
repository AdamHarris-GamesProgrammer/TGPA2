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
            var player = GameObject.FindWithTag("Player");
            if (!player) return null;
            return player.GetComponent<Inventory>();
        }

        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        public int GetSize()
        {
            return _slots.Length;
        }

        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            _slots[i].item = item;
            _slots[i].number += number;

            //Debug.Log(item.GetDisplayName() + " is placed in the " + i + "th slot");
            if (InventoryUpdated != null)
            {
                InventoryUpdated();
            }
            return true;
        }

        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (object.ReferenceEquals(_slots[i].item, item))
                {
                    
                    return true;
                }
            }
            return false;
        }

        public InventoryItem GetItemInSlot(int slot)
        {
            return _slots[slot].item;
        }

        public int GetNumberInSlot(int slot)
        {
            return _slots[slot].number;
        }

        public void RemoveFromSlot(int slot, int number)
        {
            _slots[slot].number -= number;
            if (_slots[slot].number <= 0)
            {
                _slots[slot].number = 0;
                _slots[slot].item = null;
            }
            if (InventoryUpdated != null)
            {
                InventoryUpdated();
            }
        }

        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (_slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, number); ;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

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
            int index = 0;
            foreach(InventorySlot slot in _slots)
            {
                RemoveFromSlot(index, slot.number);

                index++;
            }
        }


        public int FindItem(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        public int Find(InventoryItem item) {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (object.ReferenceEquals(_slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable)
            {
                return -1;
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (object.ReferenceEquals(_slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }


        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }
    
        object ISaveable.Save()
        {
            var slotStrings = new InventorySlotRecord[_inventorySize];
            for (int i = 0; i < _inventorySize; i++)
            {
                if (_slots[i].item != null)
                {
                    slotStrings[i].itemID = _slots[i].item.ItemID;
                    slotStrings[i].number = _slots[i].number;
                }
            }
            return slotStrings;
        }

        void ISaveable.Load(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < _inventorySize; i++)
            {
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