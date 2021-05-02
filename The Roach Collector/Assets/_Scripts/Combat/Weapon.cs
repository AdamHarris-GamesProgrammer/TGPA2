using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="InventorySystem/Weapon Config")]
public class Weapon : EquipableItem
{
    [Header("General")]
    [SerializeField] string _weaponName = "Gun";
    [SerializeField] string _weaponDescription = "Shoots things";
    [SerializeField] GameObject _gunPrefab;

    [Header("Gun")]
    [SerializeField] int _magzineCapacity = 12;
    [SerializeField] float _damage = 7.0f;
    //bullets per minute
    [Tooltip("Fire rate in BPM (Bullets Per Minute)")]
    [SerializeField] float _fireRate = 12.0f;

    [Header("Animation")]
    [SerializeField] AnimatorOverrideController _animatorOverride = null;

    public string GetName()
    {
        return _weaponName;
    }

    public GameObject GetGunObject()
    {
        return _gunPrefab;
    }

    public int GetMagazineSize()
    {
        return _magzineCapacity;
    }

    public float GetDamage()
    {
        return _damage;
    }

    public float GetFirerate()
    {
        return _fireRate;
    }

    public AnimatorOverrideController GetAnimator()
    {
        return _animatorOverride;
    }

    void Equip()
    {
        
    }

    void Unequip()
    {

    }

    void Shoot()
    {

    }
}
