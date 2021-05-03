using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("InventorySystem/Keycard"))]
public class KeycardItem : InventoryItem
{
    [Tooltip("This field relates to the type of door this key can unlock")]
    [SerializeField] private LockedDoorID _unlocksDoor;

    public LockedDoorID GetUnlockables()
    {
        return _unlocksDoor;
    }
        

}
