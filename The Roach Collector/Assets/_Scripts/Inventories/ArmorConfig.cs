using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Harris/Make New Armor")]
public class ArmorConfig : EquipableItem
{
    [Header("Armor Settings")]
    [SerializeField] private int _armor = 0;

    public int GetArmor()
    {
        return _armor;
    }

}
