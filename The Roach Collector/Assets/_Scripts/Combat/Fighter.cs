using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] float _weaponDamage = 5.0f;
    [SerializeField] float _timeBetweenAttacks = 0.1f;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _bulletSpawnPoint;

    public void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Camera.main.transform.rotation);
        bullet.GetComponent<Projectile>().SetDamage(_weaponDamage);
    }


}
