using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Combat
{
    public class PlayerFighter : Fighter
    {
        private void Start()
        {
            EquipWeapon();
        }

        public override void Shoot()
        {
            if(timer > _timeBetweenAttacks)
            {
                FireProjectile(Camera.main.transform.rotation);
                timer = 0.0f;
            }
        }
    }
}

