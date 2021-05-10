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
            if (_dockedItems.ContainsKey(index))
            {
                return _dockedItems[index].item;
            }
            return null;
        }

        public int GetNumber(int index)
        {
            if (_dockedItems.ContainsKey(index))
            {
                return _dockedItems[index].number;
            }
            return 0;
        }

        public void AddAction(InventoryItem item, int index, int number)
        {
            if (_dockedItems.ContainsKey(index))
            {  
                if (object.ReferenceEquals(item, _dockedItems[index].item))
                {
                    _dockedItems[index].number += number;
                }
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.item = item;
                slot.number = number;
                _dockedItems[index] = slot;
                //Debug.Log(slot.item.GetDisplayName() + " placed in action bar");
            }
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        public bool Use(int index, GameObject user)
        {
            if (_dockedItems.ContainsKey(index))
            {
                ActionItem action = _dockedItems[index].item as ActionItem;
                if (action)
                {
                   action.Use(user);
                    if (action.IsConsumable)
                    {
                        RemoveItems(index, 1);
                    }
                    return true;
                }

                ArmorConfig armor = _dockedItems[index].item as ArmorConfig;
                if (armor)
                {
                    armor.Use(gameObject);
                    RemoveItems(index, 1);
                }

                WeaponConfig weapon = _dockedItems[index].item as WeaponConfig;
                if(weapon)
                {
                    weapon.Use(gameObject);
                }

            }
            return false;
        }

        public void RemoveItems(int index, int number)
        {
            if (_dockedItems.ContainsKey(index))
            {
                _dockedItems[index].number -= number;
                if (_dockedItems[index].number <= 0)
                {
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
            

            if (_dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, _dockedItems[index].item))
            {
                return 0;
            }

            var actionItem = item as ActionItem;
            if (actionItem)
            {
                if (actionItem.IsConsumable)
                {
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
                AddAction(InventoryItem.GetFromID(pair.Value.itemID), pair.Key, pair.Value.number);
            }
        }
    }
}