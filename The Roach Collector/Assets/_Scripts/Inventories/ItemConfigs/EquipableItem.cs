using UnityEngine;

namespace Harris.Inventories
{
    [CreateAssetMenu(menuName = ("InventorySystem/Equipable Item"))]
    public class EquipableItem : SellableItem
    {
        [Header("Equipable Settings")]

        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation _allowedEquipLocation = EquipLocation.Helmet;


        public override void Use(GameObject user, int index)
        {
            //Gets the equipment 
            Equipment equipment = user.GetComponent<Equipment>();

            //Gets the item in the slot
            EquipableItem item = equipment.GetItemInSlot(_allowedEquipLocation);

            //Adds the item to an empty slot (if applicable)
            if(item != null)
            {
                user.GetComponent<Inventory>().AddToFirstEmptySlot(item, 1);
            }

            //Adds this item to the equipment slot
            equipment.AddItem(_allowedEquipLocation, this);
        }

        public EquipLocation GetAllowedEquipLocation()
        {
            return _allowedEquipLocation;
        }
    }
}