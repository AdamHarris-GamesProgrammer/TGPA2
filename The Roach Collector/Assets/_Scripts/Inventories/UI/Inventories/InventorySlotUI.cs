using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Harris.Inventories;
using Harris.Core.UI.Dragging;

namespace Harris.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        
        [SerializeField] InventoryItemIcon _icon = null;

        public int _index;
        InventoryItem _item;
        Inventory _inventory;


        void Awake()
        {
        }

        public void Setup(Inventory inventory, int index)
        {
            //Sets the inventory that this slot is in
            _inventory = inventory;
            //gives the slot an index
            _index = index;
            //sets the item in the icon
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

        //Adds an item to this slot
        public void AddItems(InventoryItem item, int number)
        {
            _inventory.AddItemToSlot(_index, item, number);
        }

        //Returns this item
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
    }
}