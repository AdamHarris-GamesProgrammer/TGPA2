using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public enum Weapons
    {
        Sword, 
        Axe,
        Dagger,
        Hammer,
        Staff,
    }

    public enum Potions
    {
        Health,
        Mana,
        Damage,
    }

    public enum Equipment
    {
        Apple,
    }

    public enum Armour
    {
        Helmet,
        Chest,
        Legs,
        Boots,
    }

    public enum Items
    {
        Sword,
        Axe,
        Dagger,
        Hammer,
        Staff,
        Health,
        Mana,
        Damage,
        Helmet,
        Chest,
        Legs,
        Boots,
    }

    [SerializeField] private List<Items> m_Inventory = new List<Items>();
    [SerializeField] private int m_InventoryMaxSize;

    public void AddItem(Items item)
    {
        if(m_Inventory.Count + 1 <= m_InventoryMaxSize)
        {
            m_Inventory.Add(item);
        }
        else
        {
            Debug.Log("No inventory space!");
        }
    }

    public void RemoveItem(Items item)
    {
        bool found = false;

        for(int i = 0; i < m_Inventory.Count; i++)
        {
            if(m_Inventory[i] == item)
            {
                m_Inventory.RemoveAt(i);
                found = true;
            }
        }

        if (!found)
            Debug.Log("ERROR: Couldnt find " + item + " in inventory");
    }
}
