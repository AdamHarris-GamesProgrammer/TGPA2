using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHealth : Health
{
    ActiveWeapon _weapon;
    CharacterAiming _aiming;
    CinemachineFreeLook _camera;
    Animator _animator;

    protected override void OnStart()
    {
        _weapon = GetComponent<ActiveWeapon>();
        _aiming = GetComponent<CharacterAiming>();
        _camera = FindObjectOfType<CinemachineFreeLook>();
        _animator = GetComponent<Animator>();
    }

    protected override void OnDeath()
    {
        _weapon.DropWeapon();
        _aiming.enabled = false;
        _camera.enabled = false;
        _animator.SetTrigger("isDead");
    }

    protected override void OnDamage()
    {

    }
}
