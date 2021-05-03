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
    PlayerHealthEffectController _healthEffectController;
    protected override void OnStart()
    {
        _weapon = GetComponent<ActiveWeapon>();
        _aiming = GetComponent<CharacterAiming>();
        _camera = FindObjectOfType<CinemachineFreeLook>();
        _animator = GetComponent<Animator>();
        _healthEffectController = GetComponent<PlayerHealthEffectController>();
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
        float healthPercentage = _currentHealth / _maxHealth;
        _healthEffectController.CalculateEffect(healthPercentage);
    }

    protected override void OnHeal()
    {
        float healthPercentage = _currentHealth / _maxHealth;
        _healthEffectController.CalculateEffect(healthPercentage);
    }
}
