using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapons : MonoBehaviour
{
    RaycastWeapon _currentWeapon;
    MeshSockets _sockets;
    WeaponIK _weaponIK;
    Transform _currentTarget;

    bool _isWeaponActive = false;

    [Header("Accuracy Settings")]
    [SerializeField] float _inaccuracy = 0.0f;

    [Header("Damage Settings")]
    [SerializeField] float _damageMultiplier = 0.2f;

    [SerializeField] private int _AiClipBullets = 0;
    [SerializeField] private int _AiTotalBullets = 0;
    [SerializeField] private RaycastWeapon _meleeWeapon;

    bool _usingMelee = false;


    public void SetTarget(Transform transform)
    {
        _currentTarget = transform;
        _weaponIK.SetTargetTransform(transform);
    }

    private void Awake()
    {
        _sockets = GetComponent<MeshSockets>();
        _weaponIK = GetComponent<WeaponIK>();
    }

    private void Update()
    {
        if (!_isWeaponActive) return;
        if(_currentTarget && _currentWeapon)
        {
            Vector3 target = _currentTarget.position + _weaponIK._offset;
            target += UnityEngine.Random.insideUnitSphere * _inaccuracy;
            _currentWeapon.UpdateWeapon(target);
        }

        _AiClipBullets = _currentWeapon._clipAmmo;
        _AiTotalBullets = _currentWeapon._totalAmmo;

        if (_AiClipBullets == 0 && _AiTotalBullets == 0 && !_usingMelee)
        {
            _usingMelee = true;
            Debug.Log("Change Weapon");
            RaycastWeapon meleeweapon = Instantiate(_meleeWeapon);
            EquipWeapon(meleeweapon);

            GetComponent<AIAgent>().stateMachine.ChangeState(AiStateId.Melee);

        }

        if (_currentWeapon.IsMelee && _usingMelee == false)
        {
            _usingMelee = true;
            GetComponent<AIAgent>().stateMachine.ChangeState(AiStateId.Melee);
        }


    }

    public void SetFiring(bool enabled)
    {
        //Debug.Log("Set firing: " + enabled);
        if (enabled) _currentWeapon.StartFiring();
        else _currentWeapon.StopFiring();
    }

    public void EquipWeapon(RaycastWeapon weapon)
    {

        if(_currentWeapon) Destroy(_currentWeapon.gameObject);

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
        if(_currentWeapon)
        {
            _currentWeapon.transform.SetParent(null);
            _currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            _currentWeapon.gameObject.AddComponent<Rigidbody>();
            _currentWeapon.Config.SpawnAmmo(_currentWeapon.transform.position, 10);
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
        if(_currentWeapon == null)
        {
            Debug.Log("Equipped Weapon Is Null");
        }

        return _currentWeapon;
    }

}
