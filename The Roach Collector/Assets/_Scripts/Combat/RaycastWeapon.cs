using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    [SerializeField] public WeaponConfig _weaponConfig;
    private bool _isFiring = false;
    
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private ParticleSystem _metalHitEffect;
    [SerializeField] private ParticleSystem _fleshHitEffect;

    [SerializeField] private Transform _raycastOrigin;
    
    [SerializeField] private AnimationClip _weaponAnimation;


    
    [SerializeField] public int _TotalAmno = 90;
    [SerializeField] public float _reloadDuration = 1.0f;
    [SerializeField] private float _reloadTimeLeft = 1.0f;
    [SerializeField] public bool _isReloading = false;

    public LayerMask _layerMask;
    //public PlayerUI AmnoUI;


    [SerializeField] private TrailRenderer _tracerEffect;

    public WeaponRecoil _weaponRecoil;

    public Transform GetRaycastOrigin()
    {
        return _raycastOrigin;
    }

    

    public AnimationClip GetAnimationClip()
    {
        return _weaponAnimation;
    }

    public bool IsFiring()
    {
        return _isFiring;
    }


    private void Awake()
    {
        _weaponRecoil = GetComponent<WeaponRecoil>();
    }

    public void Update()
    {
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
            }
        }
        
    }

    Ray _ray;
    RaycastHit _hitInfo;

    float _accumulatedTime;

    List<Bullet> _bullets = new List<Bullet>();

    public float _maxLifeTime = 3.0f;

    Vector3 GetPosition(Bullet bullet)
    {
        //Calculate gravity
        Vector3 gravity = Vector3.down * _weaponConfig.BulletDrop;

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

        Vector3 velocity = (target - _raycastOrigin.position).normalized * _weaponConfig.BulletSpeed;
        var bullet = CreateBullet(_raycastOrigin.position, velocity);
        _bullets.Add(bullet);

        _weaponRecoil.GenerateRecoil();
    }

    public void StartFiring()
    {
        _accumulatedTime = 0.0f;
        _isFiring = true;
        _weaponRecoil.Reset();
        //Fire();
    }

    public void StopFiring()
    {
        _isFiring = false;
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
            Vector3 p0 = GetPosition(bullet);
            bullet._time += dt;
            Vector3 p1 = GetPosition(bullet);
            RayCastSegment(p0, p1, bullet);

        });
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

            bullet._tracer.transform.position = _hitInfo.point;
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

        _accumulatedTime += deltaTime;
        UpdateBullets(deltaTime);
        
    }

    public void UpdateFiring(float deltaTime, Vector3 target)
    {
        float fireInterval = 1.0f / _weaponConfig.FireRate;
        while(_accumulatedTime > 0.0f)
        {
            for(int i = 0; i < _weaponConfig.BulletCount; ++i) {
                Fire(target += UnityEngine.Random.insideUnitSphere * _weaponConfig.WeaponSpread);
            }
            _weaponConfig.ClipAmmo--;
            //AmnoUI.UpdateAmmoUI(_clipAmmo, _clipSize, _TotalAmno);
            if (_weaponConfig.ClipAmmo <= 0)
            {
                StopFiring();
            }
            _accumulatedTime -= fireInterval;
        }
    }
}
