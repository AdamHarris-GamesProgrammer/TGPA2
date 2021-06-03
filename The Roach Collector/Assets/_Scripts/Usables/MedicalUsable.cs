using System.Collections;
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

        _user.GetComponent<Animator>().SetBool("Healing", true);
        

    }

    public override void Update(float deltaTime)
    {
        if(_applyTimer < 0.1)
        {
        }

        //Increment the apply timer
        _applyTimer += deltaTime;

        
        if(_applyTimer > _applyDuration)
        {
            //Increment the full effect timer
            _fullEffectTimer += deltaTime;

            if(_fullEffectTimer > _timeBetweenHeals)
            {
                //Reset the full effect timer
                _fullEffectTimer = 0.0f;

                //Increment the accumulated time
                _accumulatedTime += _timeBetweenHeals;


                //Heal by the amount of health this second
                _user.GetComponent<Health>().Heal(_healingAmount / _steps);

                //Remove this usable from the player if possible
                if (_accumulatedTime >= _timeForFullEffect)
                {
                    _user.GetComponent<PlayerUI>().RemoveUsable(this);
                }
            }

            //Play healing animation
            _user.GetComponent<Animator>().SetBool("Healing", false);
        }
    }

    public override float GetApplyTimeRemaining()
    {
        return _applyDuration - _applyTimer;
    }
}
