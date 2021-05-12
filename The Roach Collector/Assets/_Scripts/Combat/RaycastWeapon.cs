using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public Bullet(float time, Vector3 position, Vector3 velocity)
        {
            _time = time;
            _initialPosition = position;
            _initialVelocity = velocity;
        }
        public float _time;
        public Vector3 _initialPosition;
        public Vector3 _initialVelocity;
        public TrailRenderer _tracer;
    }

    Inventory _inventory;

    private bool _isFiring = false;
    [SerializeField] private ParticleSystem _muzzleFlash = null;
    [SerializeField] private ParticleSystem _metalHitEffect = null;
    [SerializeField] private ParticleSystem _fleshHitEffect = null;

    [SerializeField] private Transform _raycastOrigin = null;

    [SerializeField] private WeaponConfig _config;
    

    [SerializeField] private int _clipAmmo = 30;
    [SerializeField] private int _totalAmmo = 90;
    [SerializeField] private float _reloadDuration = 1.0f;
    [SerializeField] private float _reloadTimeLeft = 1.0f;
    [SerializeField] private bool _isReloading = false;
    [SerializeField] private DamageType _type;
    private float _damageMultiplier = 1.0f;

    AudioSource _audioSoruce;

    [SerializeField] private LayerMask _layerMask;

    public bool IsReloading { get { return _isReloading; } }
    public WeaponConfig Config { get { return _config; } }
    public int ClipAmmo { get { return _clipAmmo; } }
    public int TotalAmmo { get { return _totalAmmo; } set { _totalAmmo = value; } }

    float _timeSinceLastShot = 0.0f;
    float _timeBetweenShots;

    Ray _ray;
    RaycastHit _hitInfo;

    List<Bullet> _bullets = new List<Bullet>();

    private float _maxLifeTime = 3.0f;


    void Awake()
    {
        _weaponRecoil = GetComponent<WeaponRecoil>();
    }

    public void Setup()
    {
        _inventory = GetComponentInParent<Inventory>();
        _audioSoruce = GetComponent<AudioSource>();

        if (_inventory != null)
        {
            _totalAmmo = 0;
        }
        else
        {
            _clipAmmo = _config.ClipSize;
        }

        _timeBetweenShots = 1.0f / _config.FireRate;
    }

    public DamageType GetDamageType()
    {
        return _type;
    }

    public void SetDamageMultiplier(float val)
    {
        _damageMultiplier = val;
    }

    public bool NeedToReload()
    {
        return (_clipAmmo <= 0);
    }

    [SerializeField] private TrailRenderer _tracerEffect = null;

    private WeaponRecoil _weaponRecoil;
    public WeaponRecoil Recoil { get { return _weaponRecoil; } }

    public Transform GetRaycastOrigin()
    {
        return _raycastOrigin;
    }

    public float GetDamage()
    {
        return _config.Damage * _damageMultiplier;
    }

    public bool IsFiring()
    {
        return _isFiring;
    }

    public void Update()
    {
        _timeSinceLastShot += Time.deltaTime;

        if(_inventory)
        {
            if (_inventory.HasItem(_config.AmmoType))
            {
                int index = _inventory.FindItem(_config.AmmoType);

                _totalAmmo = _inventory.GetNumberInSlot(index);
            }
        }

        if (_isReloading == true)
        {
            if (_reloadTimeLeft > 0)
            {
                _reloadTimeLeft -= Time.deltaTime;
            }
            else
            {
                _reloadTimeLeft = _reloadDuration;
                _isReloading = false;

                //Add in the new bullets
                _totalAmmo += _clipAmmo;

                _audioSoruce.PlayOneShot(_config.BulletLoad);


                if (_totalAmmo < _config.ClipSize)
                {
                    _clipAmmo = _totalAmmo;
                    _totalAmmo = 0;

                    if (_inventory)
                    {
                        if (_inventory.HasItem(_config.AmmoType))
                        {
                            int index = _inventory.FindItem(_config.AmmoType);

                            _inventory.RemoveFromSlot(index, _clipAmmo);
                        }
                    }
                }
                else
                {
                    _clipAmmo = _config.ClipSize;
                    _totalAmmo -= _config.ClipSize;

                    if (_inventory)
                    {
                        if (_inventory.HasItem(_config.AmmoType))
                        {
                            int index = _inventory.FindItem(_config.AmmoType);

                            _inventory.RemoveFromSlot(index, _config.ClipSize);
                        }
                    }
                }

                _audioSoruce.PlayOneShot(_config.MagazineLoad);

                _audioSoruce.PlayOneShot(_config.SafetySwitch);
                _audioSoruce.PlayOneShot(_config.CockSound);
            }
        }
        
    }



    Vector3 GetPosition(Bullet bullet)
    {
        //Calculate gravity
        Vector3 gravity = Vector3.down * _config.BulletDrop;

        //p + v* t + 0.5 * g * t *t
        //Brackets aren't needed but they help make the equation more readable
        //First section is the initial position
        //Second section is the total displacement of the bullet
        //Third section is the amount of gravity applied to the bullet at each point
        return bullet._initialPosition + (bullet._initialVelocity * bullet._time) + (0.5f * gravity * bullet._time * bullet._time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet(0.0f, position, velocity);

        bullet._tracer = Instantiate(_tracerEffect, position, Quaternion.identity);
        bullet._tracer.AddPosition(position);

        return bullet;
    }


    private void Fire(Vector3 target)
    {
        _muzzleFlash.Emit(1);

        Vector3 velocity = (target - _raycastOrigin.position).normalized * _config.BulletSpeed;
        var bullet = CreateBullet(_raycastOrigin.position, velocity);
        _bullets.Add(bullet);

        _weaponRecoil.GenerateRecoil();

        _audioSoruce.PlayOneShot(_config.ContinuousFire);
    }

    public void StartFiring()
    {
        if(_clipAmmo <= 0)
        {
            _audioSoruce.PlayOneShot(_config.DryFire);
            Reload();
        }
        else if (_isReloading)
        {

        }
        else
        {
            _audioSoruce.PlayOneShot(_config.StartFire);
            _isFiring = true;
        }
    }

    public void StopFiring()
    {
        _isFiring = false;

        if(_config.EndFire != null)
        {
            _audioSoruce.PlayOneShot(_config.EndFire);
        }
    }

    public void UpdateBullets(float dt)
    {
        SimulateBullets(dt);
        DestroyBullets();
    }

    void DestroyBullets()
    {
        //Removes all bullets that has a life time greater than the max lifetime
        _bullets.RemoveAll(bullets => bullets._time >= _maxLifeTime);
    }

     void SimulateBullets(float dt)
    {
        _bullets.ForEach(bullet =>
        {
            //p0 origin position
            Vector3 p0 = GetPosition(bullet);
            bullet._time += dt;

            //p1 new position
            Vector3 p1 = GetPosition(bullet);

            //Calculate the raycast between the origin and new position and see if we collide with anything
            RayCastSegment(p0, p1, bullet);

        });
    }

    public void Reload()
    {
        _isReloading = true;
        _audioSoruce.PlayOneShot(_config.MagazineUnload);
    }

    void RayCastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;

        float distance = direction.magnitude;
        _ray.origin = start;
        _ray.direction = direction;

        if (Physics.Raycast(_ray, out _hitInfo, distance, _layerMask))
        {
            Rigidbody hitRb = _hitInfo.transform.gameObject.GetComponent<Rigidbody>();
            if (hitRb)
            {
                hitRb.AddForceAtPosition(_ray.direction * 20.0f, _hitInfo.point, ForceMode.Impulse);
            }

            var hitbox = _hitInfo.transform.gameObject.GetComponent<Hitbox>();
            if(hitbox)
            {
                _fleshHitEffect.transform.position = _hitInfo.point;
                _fleshHitEffect.transform.parent = _hitInfo.transform;
                _fleshHitEffect.transform.forward = _hitInfo.normal;
                _fleshHitEffect.Emit(1);


                hitbox.OnRaycastHit(this, _ray.direction);
            }
            else
            {
                _metalHitEffect.transform.position = _hitInfo.point;
                _metalHitEffect.transform.parent = _hitInfo.transform;
                _metalHitEffect.transform.forward = _hitInfo.normal;
                _metalHitEffect.Emit(1);

            }

            //TODO: Error triggered here by Tracer being destroyed before this code
            //This is not very efficient as it will be done each bullet update per bullet per frame
            //Better way is needed >: ( 
            if(bullet._tracer != null)
            {
                bullet._tracer.transform.position = _hitInfo.point;
            }

            bullet._time = _maxLifeTime;
        }
        else
        {
            if(bullet._tracer != null)
            {
                bullet._tracer.transform.position = end;
            }
        }
    }

    public void UpdateWeapon(float deltaTime, Vector3 target)
    {
        if(_isFiring)
        {
            UpdateFiring(deltaTime, target);
        }

        UpdateBullets(deltaTime);
        
    }

    

    public void UpdateFiring(float deltaTime, Vector3 target)
    {
        if(_timeSinceLastShot > _timeBetweenShots)
        {
            for (int i = 0; i < _config.BulletCount; i++)
            {

                Fire(target += UnityEngine.Random.insideUnitSphere * _config.WeaponSpread);
            }
            _clipAmmo--;

            if (_clipAmmo <= 0)
            {
                StopFiring();
            }
        }
    }
}
