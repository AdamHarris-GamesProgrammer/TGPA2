using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponConfig : MonoBehaviour
{
    [SerializeField] private int _fireRate = 25;
    [SerializeField] private float _weaponSpread = 0;
    [SerializeField] private int _bulletCount = 1;
    [SerializeField] private float _bulletSpeed = 1000.0f;
    [SerializeField] private float _bulletDrop = 0.0f;
    [SerializeField] private float _damage = 10.0f;
    [SerializeField] private int _clipAmno = 30;
    [SerializeField] private int _clipSize = 30;

    public int FireRate
    {
        get
        {
            return _fireRate;
        }
        set
        {
            _fireRate = value;
        }
    }
    public float WeaponSpread
    {
        get
        {
            return _weaponSpread;
        }
        set
        {
            _weaponSpread = value;
        }
    }
    public int BulletCount
    {
        get
        {
            return _bulletCount;
        }
        set
        {
            _bulletCount = value;
        }
    }
    public float BulletSpeed
    {
        get
        {
            return _bulletSpeed;
        }
        set
        {
            _bulletSpeed = value;
        }
    }
    public float BulletDrop
    {
        get
        {
            return _bulletDrop;
        }
        set
        {
            _bulletDrop = value;
        }
    }
    public float Damage
    {
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }
    public int ClipAmno
    {
        get
        {
            return _clipAmno;
        }
        set
        {
            _clipAmno = value;
        }
    }
    public int ClipSize
    {
        get
        {
            return _clipSize;
        }
        set
        {
            _clipSize = value;
        }
    }
}
