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
        [Min(0f)][SerializeField] protected float _timeBetweenAttacks = 0.2f;

        [SerializeField] protected Weapon _startingWeapon = null;



        Weapon _equippedWeapon;
        
        //[SerializeField] Transform _gun;

        private MeshSockets _sockets;

        GameObject _currentGunGO;

        protected float _timer = 10000f;

        private void Awake()
        {
            _sockets = GetComponent<MeshSockets>();
        }

        private void Start()
        {
            EquipWeapon(_startingWeapon);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            _equippedWeapon = newWeapon;
            //converts from bullets per minute to bullets per second
            _timeBetweenAttacks = _equippedWeapon.GetFirerate() / 60.0f;
            _timeBetweenAttacks = 1.0f / _timeBetweenAttacks;

            _currentGunGO = Instantiate(_equippedWeapon.GetGunObject(), transform);

            _sockets.Attach(_currentGunGO.transform, MeshSockets.SocketId.RightShoulder);
        }

        protected void FireProjectile(Quaternion bulletRotation)
        {
            _timer = 0.0f;
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnLocation.position, bulletRotation);
            bullet.GetComponent<Projectile>().SetDamage(_equippedWeapon.GetDamage());

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
            }

            if (audioController != null)
            {
                audioController.PlayAudio(tgpAudio.AudioType.SFX_Shoot, false);
            }
        }

        public virtual void Shoot()
        {
            if(_timer > _timeBetweenAttacks)
            {
                FireProjectile(transform.rotation);
            }

        }
    }
}

