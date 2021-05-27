using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harris.Inventories
{
    /// <summary>
    /// A ScriptableObject that represents any item that can be put in an inventory
    /// </summary>
    [CreateAssetMenu(menuName = ("InventorySystem/Item"))]
    public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("Inventory Item Settings")]
        
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] string _itemID = null;
        [Tooltip("Item name to be displayed in UI.")]
        [SerializeField] string _displayName = null;
        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] string _description = null;
        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField] Sprite _icon = null;
        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField] Pickup _pickup = null;
        [SerializeField] Pickup _ammo = null;
        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField] bool _isStackable = false;

        static Dictionary<string, InventoryItem> itemLookupCache;

        public Sprite Icon { get { return _icon; } }

        public string ItemID { get { return _itemID; } }
        public bool IsStackable { get { return _isStackable; } }
        public string Name { get { return _displayName; } }
        public virtual string Description { get { return _description; } }



        //Gets a inventory item based on the items id. 
        public static InventoryItem GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = UnityEngine.Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (item._itemID != null)
                    {
                        if (itemLookupCache.ContainsKey(item._itemID))
                        {
                            Debug.LogError(string.Format("Looks like there's a duplicate InventorySystem ID for objects: {0} and {1}", itemLookupCache[item._itemID], item));
                            continue;
                        }

                        itemLookupCache[item._itemID] = item;
                    }
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }

        //Virtual method for using this item. Not all items need a use function but it's here for when it's needed
        public virtual void Use(GameObject user, int index)
        {
            Debug.Log("Using action: " + this);
        }

        //Spawns a pickup at a specified position with a specified number of the item dropped.
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(_pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }

        public Pickup SpawnAmmo(Vector3 position, int number)
        {
            var pickup = Instantiate(_ammo);
            pickup.transform.position = position;
            pickup.Setup(_ammo.GetItem(), number);
            return pickup;
        }


        //Interface Implementation
        //Using this interface as it will create the items id when we create the asset
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(_itemID))
            {
                _itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Required by the ISerializationCallbackReciever, but we dont actually need it. 
        }
    }
}
