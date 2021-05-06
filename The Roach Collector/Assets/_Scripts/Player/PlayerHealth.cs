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

        //Armor can block 90% of incoming damage. 10% of damage will always come through
        //The remaining 90% of damage is then blocked based on how much armor the player has
        //Armor is in a range of 0 to 100

        //Calculates Armor Protection
        int armor = _equipment.GetTotalArmor();

        //Gets 10% of the damage
        float damageToGoThrough = amount / 10.0f;

        //Takes away 10% of damage from the amount
        float leftOverDamage = amount - damageToGoThrough;

        //Stores how much percent the armor should block
        float armorBlocks = 0.0f;
        if (armor > 0)
        {
            //Calculates the percentage of damage blocked
            armorBlocks = (leftOverDamage / 100) * armor;
        }

        //Left over damage is now updated to use the armor and the damage to go through
        leftOverDamage = leftOverDamage - armorBlocks + damageToGoThrough;

        Debug.Log(leftOverDamage);

        base.TakeDamage(type, leftOverDamage);
    }

    protected override void OnDeath()
    {
        _weapon.DropWeapon();
        _aiming.enabled = false;

        GetComponent<PlayerController>().AimCam.SetActive(false);
        GetComponent<PlayerController>().FollowCam.SetActive(false);


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
