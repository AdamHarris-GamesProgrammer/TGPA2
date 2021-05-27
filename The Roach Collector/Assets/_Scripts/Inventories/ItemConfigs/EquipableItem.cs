using UnityEngine;

namespace Harris.Inventories
{
    [CreateAssetMenu(menuName = ("InventorySystem/Equipable Item"))]
    public class EquipableItem : SellableItem
    {
        [Header("Equipable Settings")]
        // CONFIG DATA
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation _allowedEquipLocation = EquipLocation.Helmet;

        // PUBLIC

        public override void Use(GameObject user, int index)
        {
            Equipment equipment = user.GetComponent<Equipment>();

            EquipableItem item = equipment.GetItemInSlot(_allowedEquipLocation);

            if(item != null)
            {
                user.GetComponent<Inventory>().AddToFirstEmptySlot(item, 1);
            }

            equipment.AddItem(_allowedEquipLocation, this);
        }

        public EquipLocation GetAllowedEquipLocation()
        {
            return _allowedEquipLocation;
        }
    }
}