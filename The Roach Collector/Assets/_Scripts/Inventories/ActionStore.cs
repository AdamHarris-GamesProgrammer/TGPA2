using System;
using System.Collections.Generic;
using UnityEngine;
using Harris.Saving;

namespace Harris.Inventories
{
    public class ActionStore : MonoBehaviour, ISaveable
    {
        Dictionary<int, DockedItemSlot> _dockedItems = new Dictionary<int, DockedItemSlot>();
        private class DockedItemSlot 
        {
            public InventoryItem item;
            public int number;
        }

        public event Action storeUpdated;

        public InventoryItem GetItem(int index)
        {
            //checks if we actually have a slot for the index passed in
            if (_dockedItems.ContainsKey(index))
            {
                //Gets the item
                return _dockedItems[index].item;
            }
            return null;
        }

        public int GetNumber(int index)
        {
            //Gets the number of items in the passed in slot
            if (_dockedItems.ContainsKey(index))
            {
                return _dockedItems[index].number;
            }
            return 0;
        }

        public void AddItem(InventoryItem item, int index, int number)
        {
            //if we already have an index for the index
            if (_dockedItems.ContainsKey(index))
            {  
                //if the object in the action slot is the same as the passed in item
                if (object.ReferenceEquals(item, _dockedItems[index].item))
                {
                    //Then add the number of items to this slot
                    _dockedItems[index].number += number;
                }
            }
            //We have not got this slot filled
            else
            {
                //Create a slot instance and set it up
                var slot = new DockedItemSlot();
                slot.item = item;
                slot.number = number;
                _dockedItems[index] = slot;
                //Debug.Log(slot.item.GetDisplayName() + " placed in action bar");
            }

            //Update the action bar
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        public bool Use(int index, GameObject user)
        {
            //Checks we have an item in this slot
            if (_dockedItems.ContainsKey(index))
            {
                //If the item is an action item
                ActionItem action = _dockedItems[index].item as ActionItem;
                if (action)
                {
                   action.Use(user, index);
                    if (action.IsConsumable)
                    {
                        RemoveItems(index, 1);
                    }
                    return true;
                }

                //if the item is an armor item
                ArmorConfig armor = _dockedItems[index].item as ArmorConfig;
                if (armor)
                {
                    armor.Use(gameObject, index);
                    RemoveItems(index, 1);
                }

                //if the item is a weapon
                WeaponConfig weapon = _dockedItems[index].item as WeaponConfig;
                if(weapon)
                {
                    weapon.Use(gameObject, index);
                }

            }
            return false;
        }

        public void RemoveItems(int index, int number)
        {
            //If we have this index
            if (_dockedItems.ContainsKey(index))
            {
                //remove the amount of items we are using
                _dockedItems[index].number -= number;

                //if there is now 0 items in the slot 
                if (_dockedItems[index].number <= 0)
                {
                    //then remove the item
                    _dockedItems.Remove(index);
                }
                if (storeUpdated != null)
                {
                    storeUpdated();
                }
            }
            
        }

        public int MaxAcceptable(InventoryItem item, int index)
        {
            //if we have an item here and the item is not the same as the passed in item
            if (_dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, _dockedItems[index].item))
            {
                return 0;
            }

            //if the item is an action item
            var actionItem = item as ActionItem;
            if (actionItem)
            {
                //if it is consumable
                if (actionItem.IsConsumable)
                {
                    //return the max value
                    return int.MaxValue;
                }
            }

            if (_dockedItems.ContainsKey(index))
            {
                return 0;
            }

            return 1;
        }


        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
        }

        object ISaveable.Save()
        {
            var state = new Dictionary<int, DockedItemRecord>();
            foreach (var pair in _dockedItems)
            {
                var record = new DockedItemRecord();
                record.itemID = pair.Value.item.ItemID;
                record.number = pair.Value.number;
                state[pair.Key] = record;
            }
            return state;
        }

        void ISaveable.Load(object state)
        {
            var stateDict = (Dictionary<int, DockedItemRecord>)state;
            foreach (var pair in stateDict)
            {
                AddItem(InventoryItem.GetFromID(pair.Value.itemID), pair.Key, pair.Value.number);
            }
        }
    }
}