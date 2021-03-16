using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon _weaponPrefab;


    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();

        if(activeWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(_weaponPrefab);
            activeWeapon.Equip(newWeapon);
        }
    }
}
