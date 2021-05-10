using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Harris.Core.UI.Dragging;
using Harris.Inventories;

namespace Harris.UI.Inventories
{
    /// <summary>
    /// An slot for the players equipment.
    /// </summary>
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA

        [SerializeField] InventoryItemIcon _itemIcon = null;
        [SerializeField] EquipLocation _equipLocation = EquipLocation.Helmet;

        // CACHE
        Equipment playerEquipment;

        // LIFECYCLE METHODS
       
        public EquipLocation GetEquipLocation()
        {
            return _equipLocation;
        }

        private void Awake() 
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;

            playerEquipment = player.GetComponent<Equipment>();
            playerEquipment.EquipmentUpdated += RedrawUI;
        }

        private void Start() 
        {
            RedrawUI();
        }

        // PUBLIC

        public int MaxAcceptable(InventoryItem item)
        {
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedEquipLocation() != _equipLocation) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }

        public void AddItems(InventoryItem item, int number)
        {
            playerEquipment.AddItem(_equipLocation, (EquipableItem) item);
        }

        public InventoryItem GetItem()
        {
            return playerEquipment.GetItemInSlot(_equipLocation);
        }

        public int GetNumber()
        {
            if (GetItem() != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void RemoveItems(int number)
        {
            playerEquipment.RemoveItem(_equipLocation);
        }

        // PRIVATE

        void RedrawUI()
        {
            _itemIcon.SetItem(playerEquipment.GetItemInSlot(_equipLocation));
        }
    }
}