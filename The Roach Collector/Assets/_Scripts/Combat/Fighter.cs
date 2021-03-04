using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using tgpAudio;

namespace TGP.Combat
{
    public class Fighter : MonoBehaviour
    {
        public AudioController audioController;
        
 

        [Header("Bullet Properties")]
        [Tooltip("The bullet object that will spawn from the gun")]
        [SerializeField] private GameObject _bulletPrefab;
        [Tooltip("The spawn location for bullets, this should be the end of the gun")]
        [SerializeField] private Transform _bulletSpawnLocation;
        [Tooltip("The amount of damage that the launched projectile will do")]
        [Min(0f)][SerializeField] float _weaponDamage = 5.0f;
        [Tooltip("The time between each shot, acts as a basic fire rate property")]
        [Min(0f)][SerializeField] float _timeBetweenAttacks = 0.2f;

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


                GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnLocation.position, transform.rotation);
                bullet.GetComponent<Projectile>().SetDamage(_weaponDamage);
                audioController.PlayAudio(tgpAudio.AudioType.SFX_Shoot, false);

            }
            
        }
    }
}

