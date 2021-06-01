﻿using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class MedicalUsable : UsableItem
{
    readonly float _applyDuration;
    readonly float _timeForFullEffect;
    readonly float _healingAmount;

    float _applyTimer = 0.0f;
    float _fullEffectTimer = 0.0f;

    readonly float _timeBetweenHeals = 0.0f;

    float _accumulatedTime = 0.0f;
    readonly int _steps = 5;
    

    public MedicalUsable(GameObject user, float timeToApply, float timeForFullEffect, float healingAmount)
    {
        _user = user;
        _applyDuration = timeToApply;
        _timeForFullEffect = timeForFullEffect;
        _healingAmount = healingAmount;

        _timeBetweenHeals = _timeForFullEffect / _steps;

        _id = UsableID.MEDKIT;
    }

    public override void Update(float deltaTime)
    {
        _applyTimer += deltaTime;

        //TODO: Add some kind of animation for applying bandages
        
        if(_applyTimer > _applyDuration)
        {
            _fullEffectTimer += deltaTime;

            if(_fullEffectTimer > _timeBetweenHeals)
            {
                _fullEffectTimer = 0.0f;
                _accumulatedTime += _timeBetweenHeals;

                _user.GetComponent<Health>().Heal(_healingAmount / _steps);

                if (_accumulatedTime >= _timeForFullEffect)
                {
                    //TODO: Can enemies use these medkits or not?
                    //TODO: if they can this whole system will need to be reworked including the PlayerController/AIAgent classes

                    _user.GetComponent<PlayerUI>().RemoveUsable(this);
                }

            }
        }
    }

    public override float GetApplyTimeRemaining()
    {
        return _applyDuration - _applyTimer;
    }
}
