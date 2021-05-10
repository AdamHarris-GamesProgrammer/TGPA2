using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Harris.Inventories;
using Harris.Core.UI.Dragging;

namespace Harris.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon _icon = null;
        [SerializeField] Sprite _selectedIcon = null;
        Sprite defaultIcon;

        // STATE
        public int _index;
        InventoryItem _item;
        Inventory _inventory;

        // PUBLIC

        void Awake()
        {
            defaultIcon = GetComponent<Image>().sprite;
        }

        public void Setup(Inventory inventory, int index)
        {
            this._inventory = inventory;
            this._index = index;
            _icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        public int MaxAcceptable(InventoryItem item)
        {
            if (_inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(InventoryItem item, int number)
        {
            _inventory.AddItemToSlot(_index, item, number);
        }

        public InventoryItem GetItem()
        {
            return _inventory.GetItemInSlot(_index);
        }

        public int GetNumber()
        {
            return _inventory.GetNumberInSlot(_index);
        }

        public void RemoveItems(int number)
        {
            _inventory.RemoveFromSlot(_index, number);
        }

        public void SetSelected(bool isSelected)
        {
            Image image = GetComponent<Image>();
            if (isSelected)
            {
                image.sprite = _selectedIcon;
            }
            else
            {
                image.sprite = defaultIcon;
            }

        }
    }
}