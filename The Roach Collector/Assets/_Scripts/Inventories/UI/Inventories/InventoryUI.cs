using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Inventories;

namespace Harris.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventorySlotUI _inventoryItemPrefab = null;

        Inventory _inventory;


        private void Awake() 
        {
            Inventory parentInventory = GetComponentInParent<Inventory>();

            //if we have an inventory as our parent
            if (parentInventory)
            {
                //this is a chest
                _inventory = parentInventory;
            }
            else
            {
                //this is the player
                _inventory = Inventory.GetPlayerInventory();
            }

            //Add the redraw method to the inventory update method
            _inventory.InventoryUpdated += Redraw;
        }

        private void Start()
        {
            //Redraws the inventory on start
            Redraw();
        }

        private void Redraw()
        {
            //Deletes all the slots he have
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            //loops through the slots in our inventory
            for (int i = 0; i < _inventory.GetSize(); i++)
            {
                //Instantiate a new slot and set the name and icon etc. 
                var itemUI = Instantiate(_inventoryItemPrefab, transform);
                itemUI.name = i.ToString();
                itemUI.Setup(_inventory, i);
            }
        }
    }
}