using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float _weaponDamage = 5.0f;
        [SerializeField] float _timeBetweenAttacks = 0.2f;
        [SerializeField] GameObject _bulletPrefab;
        [SerializeField] Transform _bulletSpawnPoint;

        float timer = 10000f;

        private void Update()
        {
            timer += Time.deltaTime;
        }

        public void Shoot()
        {
            if(timer > _timeBetweenAttacks)
            {
                timer = 0.0f;


                GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, transform.rotation);
                bullet.GetComponent<Projectile>().SetDamage(_weaponDamage);
            }
            
        }
    }
}

