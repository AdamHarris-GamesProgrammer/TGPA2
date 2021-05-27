using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("InventorySystem/Weapon Config"))]
public class WeaponConfig : EquipableItem
{
    [Header("Weapon Settings")]
    [SerializeField] private int _fireRate = 25;
    [SerializeField] private float _weaponSpread = 0;
    [SerializeField] private int _bulletCount = 1;
    [SerializeField] private float _bulletSpeed = 1000.0f;
    [SerializeField] private float _bulletDrop = 0.0f;
    [SerializeField] private float _damage = 10.0f;

    [SerializeField] private int _clipSize = 30;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private float _reloadDuration = 1.0f;
    [SerializeField] private RaycastWeapon _weapon;
    [SerializeField] private AmmoItem _ammoItem;
    [SerializeField] private bool _isAutomatic = false;
    

    [Header("Sound Settings")]  
    [SerializeField] private AudioClip _dryFire = null;         //
    [SerializeField] private AudioClip _startFire = null;       //
    [SerializeField] private AudioClip _magazineUnload = null;  //
    [SerializeField] private AudioClip _magazineLoad = null;    //
    [SerializeField] private AudioClip _safetySwitch = null;
    [SerializeField] private AudioClip _bulletLoad = null;      //
    [SerializeField] private AudioClip _cockSound = null;
    [SerializeField] private AudioClip _endFire = null;         //
    [SerializeField] private AudioClip _continuousFire = null;  //

    public AudioClip StartFire { get { return _startFire; } }
    public AudioClip ContinuousFire{get { return _continuousFire; }}
    public AudioClip EndFire{get { return _endFire; }}
    public AudioClip CockSound{get { return _cockSound; }}
    public AudioClip SafetySwitch{get { return _safetySwitch; }}
    public AudioClip BulletLoad{get { return _bulletLoad; }}

    public AudioClip MagazineLoad{get { return _magazineLoad; }}
    public AudioClip MagazineUnload{get { return _magazineUnload; }}

    public AudioClip DryFire { get { return _dryFire; } }

    public AmmoItem AmmoType { get { return _ammoItem; } }
    public int FireRate { get { return _fireRate; } set { _fireRate = value; } }
    public float WeaponSpread { get { return _weaponSpread; } set { _weaponSpread = value; } }
    public int BulletCount { get { return _bulletCount; } set { _bulletCount = value; } }
    public float BulletSpeed { get { return _bulletSpeed; } set { _bulletSpeed = value; } }
    public float BulletDrop { get { return _bulletDrop; } set { _bulletDrop = value; } }
    public float Damage { get { return _damage; } set { _damage = value; } }
    public int ClipSize { get { return _clipSize; } set { _clipSize = value; } }

    public bool IsAutomatic { get { return _isAutomatic; } }
    public DamageType DamageType { get { return _damageType; } }
    public float ReloadDuration { get { return _reloadDuration; } }

    public override void Use(GameObject user, int index)
    {
        if (user.CompareTag("Player"))
        {
            RaycastWeapon weapon = Instantiate(_weapon);
            user.GetComponent<ActiveWeapon>().Equip(weapon);
        }
    }
}
