using System;
using UnityEngine;

namespace Harris.Inventories
{
    [CreateAssetMenu(menuName = ("InventorySystem/Action Item"))]
    public class ActionItem : SellableItem
    {
        // CONFIG DATA
        [Tooltip("Does an instance of this item get consumed every time it's used.")]
        [SerializeField] bool _isConsumable = false;

        // PUBLIC

        public bool IsConsumable { get { return _isConsumable; } }
    }
}