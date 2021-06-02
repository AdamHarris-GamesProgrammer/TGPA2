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
        Debug.Log(amount);
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
        //Calculates the amount of damage we take after our armors resistance
        amount *= resistance;
        Debug.Log(amount);

        base.TakeDamage(type, amount);
    }

    protected override void OnDeath()
    {
        //Drops our wepaon
        _weapon.DropWeapon();
        _aiming.enabled = false;

        //Disables our cameras
        GetComponent<PlayerController>().AimCam.SetActive(false);
        GetComponent<PlayerController>().FollowCam.SetActive(false);

        //Sets the dying animation trigger
        _animator.SetTrigger("isDead");
        //Enables the CRT camera and death screen
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
