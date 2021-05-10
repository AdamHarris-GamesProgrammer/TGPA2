using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Inventories;

namespace Harris.UI.Inventories
{
    /// <summary>
    /// To be placed on the root of the inventory UI. Handles spawning all the
    /// inventory slot prefabs.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] InventorySlotUI _inventoryItemPrefab = null;

        // CACHE
        Inventory _playerInventory;

        // LIFECYCLE METHODS

        private void Awake() 
        {
            Inventory parent = GetComponentInParent<Inventory>();

            if (parent)
            {
                _playerInventory = parent;
            }
            else
            {
                _playerInventory = Inventory.GetPlayerInventory();
            }

            _playerInventory.InventoryUpdated += Redraw;
        }

        private void Start()
        {
            Redraw();
        }

        // PRIVATE

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _playerInventory.GetSize(); i++)
            {
                var itemUI = Instantiate(_inventoryItemPrefab, transform);
                itemUI.name = i.ToString();
                itemUI.Setup(_playerInventory, i);
            }
        }
    }
}