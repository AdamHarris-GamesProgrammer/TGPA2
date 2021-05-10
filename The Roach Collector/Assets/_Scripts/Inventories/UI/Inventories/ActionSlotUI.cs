using System.Collections;
using System.Collections.Generic;
using Harris.Core.UI.Dragging;
using Harris.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace Harris.UI.Inventories
{
    /// <summary>
    /// The UI slot for the player action bar.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon _icon = null;
        [SerializeField] public int _index = 0;
        [SerializeField] Text _textObject;

        // CACHE
        ActionStore store;

        // LIFECYCLE METHODS
        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;
            store = player.GetComponent<ActionStore>();
            if (!store) return;
            store.storeUpdated += UpdateIcon;

            //Hotkey number above the slot.
            _textObject.text = string.Format("{0}", _index + 1);
        }

        // PUBLIC

        public InventoryItem GetItem()
        {
            return store.GetItem(_index);
        }

        public void AddItems(InventoryItem item, int number)
        {
            store.AddAction(item, _index, number);
        }

        public int GetNumber()
        {
            return store.GetNumber(_index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return store.MaxAcceptable(item, _index);
        }

        public void RemoveItems(int number)
        {
            store.RemoveItems(_index, number);
        }

        // PRIVATE

        void UpdateIcon()
        {
            _icon.SetItem(GetItem(), GetNumber());
        }
    }
}
