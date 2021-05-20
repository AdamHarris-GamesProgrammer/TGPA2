using System.Collections;
using System.Collections.Generic;
using Harris.Core.UI.Dragging;
using Harris.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace Harris.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon _icon = null;
        [SerializeField] public int _index = 0;
        [SerializeField] Text _textObject;

        ActionStore _store;

        private void Awake()
        {
            //find the player
            var player = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;

            //find the action store
            _store = player.GetComponent<ActionStore>();
            if (!_store) return;
            _store.storeUpdated += UpdateIcon;

            //setup the Hotkey number above the slot.
            _textObject.text = string.Format("{0}", _index + 1);
        }


        //Returns the item at the index of this action bar
        public InventoryItem GetItem()
        {
            return _store.GetItem(_index);
        }

        //Adds an item the slot
        public void AddItems(InventoryItem item, int number)
        {
            _store.AddItem(item, _index, number);
        }

        //Gets the number of items in this location
        public int GetNumber()
        {
            return _store.GetNumber(_index);
        }

        //Gets the maximum amount of items allowed in this slot
        public int MaxAcceptable(InventoryItem item)
        {
            return _store.MaxAcceptable(item, _index);
        }

        //Remove X amount of items from this slot
        public void RemoveItems(int number)
        {
            _store.RemoveItems(_index, number);
        }


        //Updates the icon for the slot
        void UpdateIcon()
        {
            _icon.SetItem(GetItem(), GetNumber());
        }
    }
}
