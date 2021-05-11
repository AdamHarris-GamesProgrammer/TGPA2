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

    [Header("Accuracy Settinga")]
    [SerializeField] float _inaccuracy = 0.0f;

    [Header("Damage Settings")]
    [SerializeField] float _damageMultiplier = 0.2f;

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
            _currentWeapon.UpdateWeapon(Time.deltaTime, target);
        }
    }

    public void SetFiring(bool enabled)
    {
        if (enabled)
        {
            _currentWeapon.StartFiring();
        }else
        {
            _currentWeapon.StopFiring();
        }
    }

    public void EquipWeapon(RaycastWeapon weapon)
    {
        _currentWeapon = weapon;
        _currentWeapon.transform.SetParent(transform, false);
        _sockets.Attach(weapon.transform, MeshSockets.SocketID.RightHand);
        _weaponIK.SetAimTransform(_currentWeapon.GetRaycastOrigin());
        _weaponIK.SetWeaponTransform(_currentWeapon.transform);
        _isWeaponActive = true;
        _currentWeapon.SetDamageMultiplier(_damageMultiplier);
        _currentWeapon.Setup();
    }

    public void DropWeapon()
    {
        if(_currentWeapon)
        {
            _currentWeapon.transform.SetParent(null);
            _currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            _currentWeapon.gameObject.AddComponent<Rigidbody>();
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
