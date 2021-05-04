using UnityEngine;

namespace Harris.Inventories
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = ("InventorySystem/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation _allowedEquipLocation = EquipLocation.Weapon;

        // PUBLIC

        public override void Use(GameObject user)
        {
            user.GetComponent<Equipment>().AddItem(_allowedEquipLocation, this);
        }

        public EquipLocation GetAllowedEquipLocation()
        {
            return _allowedEquipLocation;
        }
    }
}