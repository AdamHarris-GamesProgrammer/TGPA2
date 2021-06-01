using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Harris.Inventories;
using TGP.Control;

public class PlayerHealth : Health
{
    ActiveWeapon _weapon;
    CharacterAiming _aiming;
    Animator _animator;
    PlayerHealthEffectController _healthEffectController;
    Equipment _equipment;
    PlayerController _player;

    [SerializeField] GameObject _CRTCamera;
    [SerializeField] GameObject _deathScreenUI;


    protected override void OnStart()
    {
        _weapon = GetComponent<ActiveWeapon>();
        _aiming = GetComponent<CharacterAiming>();
        _animator = GetComponent<Animator>();
        _healthEffectController = GetComponent<PlayerHealthEffectController>();
        _equipment = GetComponent<Equipment>();
        _player = GetComponent<PlayerController>();
    }

    public override void TakeDamage(DamageType type, float amount)
    {
        //Stops the Character from taking damage if they don't need to.
        if (_isDead) return;
        if (!_canBeHarmed) return;

        float resistance = 1.0f;
        switch (type)
        {
            case DamageType.PROJECTILE_DAMAGE:
                resistance -= _player.GetStat(StatID.PROJECTILE_RESISTANCE)._value;
                break;
            case DamageType.MELEE_DAMGE:
                resistance -= _player.GetStat(StatID.MELEE_RESISTANCE)._value;
                break;
        }
        amount *= resistance;

        base.TakeDamage(type, amount);
    }

    protected override void OnDeath()
    {
        _weapon.DropWeapon();
        _aiming.enabled = false;

        GetComponent<PlayerController>().AimCam.SetActive(false);
        GetComponent<PlayerController>().FollowCam.SetActive(false);


        _animator.SetTrigger("isDead");
        _CRTCamera.SetActive(true);
        _deathScreenUI.SetActive(true);
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
