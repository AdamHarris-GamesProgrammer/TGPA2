using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Harris.Core.UI.Dragging;
using Harris.Inventories;

namespace Harris.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon _itemIcon = null;
        [SerializeField] EquipLocation _equipLocation = EquipLocation.Helmet;

        Equipment _playerEquipment;

        public EquipLocation EquipLocation {  get { return _equipLocation; } }

        private void Awake() 
        {
            //Finds our player
            var player = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;

            //Gets the equipment component from the player
            _playerEquipment = player.GetComponent<Equipment>();
            //Sets the equipment to be redrawn when needed
            _playerEquipment.EquipmentUpdated += RedrawUI;
        }

        private void Start() 
        {
            //Redraws the UI when needed
            RedrawUI();
        }


        public int MaxAcceptable(InventoryItem item)
        {
            //Is this an equippable item
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;

            //if the locations are different
            if (equipableItem.GetAllowedEquipLocation() != _equipLocation) return 0;

            //if we have no item
            if (GetItem() != null) return 0;

            return 1;
        }

        public void AddItems(InventoryItem item, int number)
        {
            //Adds an item to the equipment
            _playerEquipment.AddItem(_equipLocation, (EquipableItem) item);
        }

        public InventoryItem GetItem()
        {
            //Returns an item from the equip location
            return _playerEquipment.GetItemInSlot(_equipLocation);
        }

        public int GetNumber()
        {
            //return the number of items in this slot
            if (GetItem() != null) return 1;
            else return 0;
        }

        public void RemoveItems(int number)
        {
            _playerEquipment.RemoveItem(_equipLocation);
        }


        void RedrawUI()
        {
            _itemIcon.SetItem(_playerEquipment.GetItemInSlot(_equipLocation));
        }
    }
}