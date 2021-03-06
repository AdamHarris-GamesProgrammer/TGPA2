using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Combat
{
    public class PlayerFighter : Fighter
    {
        private void Start()
        {
            Debug.Log("Equipping Weapon");
            EquipWeapon();
        }

        public override void Shoot()
        {
            Debug.Log("Player Fighter");
            if(timer > _timeBetweenAttacks)
            {
                FireProjectile(Camera.main.transform.rotation);
                timer = 0.0f;
            }
        }
    }
}

