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


    private bool _isFiring = false;
    [SerializeField] private int _fireRate = 25;
    [SerializeField] private float _bulletSpeed = 1000.0f;
    [SerializeField] private float _bulletDrop = 0.0f;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private Transform _raycastOrigin;

    [SerializeField] private AnimationClip _weaponAnimation;
    


    [SerializeField] private TrailRenderer _tracerEffect;

    public WeaponRecoil _weaponRecoil;

    private Transform _raycastTarget;

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


    public void SetRaycastTarget(Transform newTransform)
    {
        _raycastTarget = newTransform;
    }

    Ray _ray;
    RaycastHit _hitInfo;

    float _accumulatedTime;

    List<Bullet> _bullets = new List<Bullet>();

    public float _maxLifeTime = 3.0f;

    Vector3 GetPosition(Bullet bullet)
    {
        //Calculate gravity
        Vector3 gravity = Vector3.down * _bulletDrop;

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


    private void Fire()
    {
        _muzzleFlash.Emit(1);

        Vector3 velocity = (_raycastTarget.position - _raycastOrigin.position).normalized * _bulletSpeed;
        var bullet = CreateBullet(_raycastOrigin.position, velocity);
        _bullets.Add(bullet);

        _weaponRecoil.GenerateRecoil();
    }

    public void StartFiring()
    {
        _accumulatedTime = 0.0f;
        _isFiring = true;
        _weaponRecoil.Reset();
        Fire();
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

        if (Physics.Raycast(_ray, out _hitInfo, distance))
        {
            _hitEffect.transform.position = _hitInfo.point;
            _hitEffect.transform.parent = _hitInfo.transform;
            _hitEffect.transform.forward = _hitInfo.normal;
            _hitEffect.Emit(1);


            Rigidbody hitRb = _hitInfo.transform.gameObject.GetComponent<Rigidbody>();
            if (hitRb)
            {
                hitRb.AddForceAtPosition(new Vector3(0.0f, 0.0f, 5.0f), _hitInfo.point, ForceMode.Impulse);
            }

            bullet._tracer.transform.position = _hitInfo.point;
            bullet._time = _maxLifeTime;
        }
        else
        {
            bullet._tracer.transform.position = end;
        }
    }

    public void UpdateFiring(float deltaTime)
    {
        _accumulatedTime += deltaTime;

        float fireInterval = 1.0f / _fireRate;
        while(_accumulatedTime > 0.0f)
        {
            Fire();
            _accumulatedTime -= fireInterval;
        }
    }
}
