﻿using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class DamageResistanceUsable : UsableItem
{
    float _applyDuration = 0.0f;
    float _applyTimer = 0.0f;
    float _effectDuration = 0.0f;
    float _effectTimer = 0.0f;
    float _effectAmount = 0.0f;

    bool _isApplied = false;

    public DamageResistanceUsable(GameObject user, float applyTime, float duration, float amount)
    {
        _user = user;
        _id = UsableID.RESISTANCE;

        _applyDuration = applyTime;
        _effectDuration = duration;
        _effectAmount = amount;
    }

    public override void Update(float deltaTime)
    {
        _applyTimer += deltaTime;

        if(_applyTimer > _applyDuration)
        {
            if(!_isApplied)
            {
                _isApplied = true;
                _user.GetComponent<PlayerController>().EquipStat(new StatValues(StatID.DAMAGE_RESISTANCE, _effectAmount));
            }

            _effectTimer += Time.deltaTime;

            if(_effectTimer > _effectDuration)
            {
                _user.GetComponent<PlayerController>().UnequipStat(new StatValues(StatID.DAMAGE_RESISTANCE, _effectAmount));
                _user.GetComponent<PlayerController>().RemoveUsable(this);
            }
        }
    }

    public override float GetApplyTimeRemaining()
    {
        return _applyDuration - _applyTimer;
    }
}