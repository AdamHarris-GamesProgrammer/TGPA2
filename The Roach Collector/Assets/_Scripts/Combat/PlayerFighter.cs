using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Combat
{
    public class PlayerFighter : Fighter
    {
        private void Start()
        {
            EquipWeapon(_startingWeapon);
        }

        public override void Shoot()
        {
            if(_timer > _timeBetweenAttacks)
            {
                FireProjectile(Camera.main.transform.rotation);
                _timer = 0.0f;
            }
        }
    }
}
