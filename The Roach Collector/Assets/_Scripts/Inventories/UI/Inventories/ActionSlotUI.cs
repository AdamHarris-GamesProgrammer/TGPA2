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
        ActionStore _store;

        // LIFECYCLE METHODS
        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;
            _store = player.GetComponent<ActionStore>();
            if (!_store) return;
            _store.storeUpdated += UpdateIcon;

            //Hotkey number above the slot.
            _textObject.text = string.Format("{0}", _index + 1);
        }

        // PUBLIC

        public InventoryItem GetItem()
        {
            return _store.GetItem(_index);
        }

        public void AddItems(InventoryItem item, int number)
        {
            _store.AddAction(item, _index, number);
        }

        public int GetNumber()
        {
            return _store.GetNumber(_index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return _store.MaxAcceptable(item, _index);
        }

        public void RemoveItems(int number)
        {
            _store.RemoveItems(_index, number);
        }

        // PRIVATE

        void UpdateIcon()
        {
            _icon.SetItem(GetItem(), GetNumber());
        }
    }
}
