using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHealth : Health
{
    ActiveWeapon _weapon;
    CharacterAiming _aiming;
    CinemachineFreeLook _camera;

    protected override void OnStart()
    {
        _weapon = GetComponent<ActiveWeapon>();
        _aiming = GetComponent<CharacterAiming>();
        _camera = FindObjectOfType<CinemachineFreeLook>();
    }

    protected override void OnDeath()
    {
        _weapon.DropWeapon();
        _aiming.enabled = false;
        _camera.enabled = false;
    }

    protected override void OnDamage()
    {

    }
}
