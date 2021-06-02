using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWeapons : MonoBehaviour
{
    RaycastWeapon _currentWeapon;
    MeshSockets _sockets;
    WeaponIK _weaponIK;
    Transform _currentTarget;

    bool _isWeaponActive = false;

    [Header("Accuracy Settings")]
    [SerializeField] float _inaccuracy = 0.0f;

    [Header("Melee Weapon")]
    [SerializeField] private RaycastWeapon _meleeWeapon;


    [Header("Damage Settings")]
    [SerializeField] float _damageMultiplier = 0.2f;
    [Header("Debugging")]
    [SerializeField] private int _AiClipBullets = 0;
    [SerializeField] private int _AiTotalBullets = 0;

    bool _usingMelee = false;
    AIHealth _health;


    public void SetTarget(Transform transform)
    {
        _currentTarget = transform;
        _weaponIK.SetTargetTransform(transform);
    }

    private void Awake()
    {
        _sockets = GetComponent<MeshSockets>();
        _weaponIK = GetComponent<WeaponIK>();
        _health = GetComponent<AIHealth>();
    }

    private void Update()
    {
        if (_health.IsDead) return;
        if (!_isWeaponActive) return;
        if (!_currentWeapon) return;

        //Generates a target to shoot at with a slight bit of added inaccuracy 
        if(_currentTarget && _currentWeapon)
        {
            Vector3 target = _currentTarget.position + _weaponIK._offset;
            target += UnityEngine.Random.insideUnitSphere * _inaccuracy;
            _currentWeapon.UpdateWeapon(target);
        }

        //Stores the current bullets
        _AiClipBullets = _currentWeapon.ClipAmmo;
        _AiTotalBullets = _currentWeapon.TotalAmmo;

        //Should the AI switch to melee
        if (_AiClipBullets == 0 && _AiTotalBullets == 0 && !_usingMelee)
        {
            //Melee
            _usingMelee = true;

            //Instantiate the weapon
            RaycastWeapon meleeweapon = Instantiate(_meleeWeapon);
            //Equip the weapon
            EquipWeapon(meleeweapon);
            //Change to melee state
            if(GetComponent<AIAgent>().Aggrevated) GetComponent<AIAgent>().stateMachine.ChangeState(AiStateId.Melee);

        }
    }

    public void SetFiring(bool enabled)
    {
        //Debug.Log("Set firing: " + enabled);
        if (enabled) _currentWeapon?.StartFiring();
        else _currentWeapon?.StopFiring();
    }

    public void EquipWeapon(RaycastWeapon weapon)
    {
        //Destroys the current weapon if there is one
        if(_currentWeapon) Destroy(_currentWeapon.gameObject);

        //Sets all the details for the current weapon
        _currentWeapon = weapon;
        _currentWeapon.transform.SetParent(transform, false);
        _sockets.Attach(weapon.transform, MeshSockets.SocketID.RightHand);
        _weaponIK.SetAimTransform(_currentWeapon.RaycastOrigin);
        _weaponIK.SetWeaponTransform(_currentWeapon.transform);
        _isWeaponActive = true;
        _currentWeapon.DamageMultiplier = _damageMultiplier;
        _currentWeapon.Setup();
    }

    public void DropWeapon()
    {
        //Only runs if we have a weapon to drop
        if(_currentWeapon)
        {
            //removes the parentness
            _currentWeapon.transform.SetParent(null);
            //Enables the collider
            _currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            //Adds a rigidbody
            if (_currentWeapon.gameObject.GetComponent<Rigidbody>()) Destroy(_currentWeapon.gameObject);
                
                _currentWeapon.gameObject.AddComponent<Rigidbody>();

            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position, out hit, 5.0f, ~0);
            //Spawns ammo for the gun, if applicable
            if(!_currentWeapon.IsMelee) _currentWeapon.Config.SpawnAmmo(hit.position, 10);
            //Sets the current weapon to null
            _currentWeapon = null;
            _weaponIK.SetWeaponTransform(null);
        }
    }

    public bool HasWeapon()
    {
        return _currentWeapon != null;
    }

    public RaycastWeapon GetEquippedWeapon()
    {
        if(_currentWeapon == null) Debug.Log("Equipped Weapon Is Null");

        return _currentWeapon;
    }

    void Stabbing()
    {
        GetComponent<WeaponStabCheck>().SetStabbing(true);
    }

    void NotStabbing()
    {
        GetComponent<WeaponStabCheck>().SetStabbing(false);
    }
}
