using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon _weaponPrefab;
    public bool destroyPickup = true;


    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();

        if(activeWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(_weaponPrefab);
            activeWeapon.Equip(newWeapon);

            if (destroyPickup) Destroy(gameObject);
        }

        Hitbox hitbox = other.gameObject.GetComponent<Hitbox>();

        if (hitbox)
        {
            AIWeapons weapons = other.gameObject.GetComponent<AIWeapons>();
            if(weapons != null)
            {
                if (weapons.HasWeapon()) return;

                RaycastWeapon newWeapon = Instantiate(_weaponPrefab);
                weapons.EquipWeapon(newWeapon);

                if (destroyPickup) Destroy(gameObject);
            }
        }

            
        
    }
}
