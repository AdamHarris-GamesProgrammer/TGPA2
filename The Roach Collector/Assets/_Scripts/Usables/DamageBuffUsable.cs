using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class DamageBuffUsable : UsableItem
{
    float _applyDuration = 0.0f;
    float _applyTimer = 0.0f;
    float _effectDuration = 0.0f;
    float _effectTimer = 0.0f;
    float _effectAmount = 0.0f;

    bool _isApplied = false;

    public DamageBuffUsable(GameObject user, float applyTime, float duration, float amount)
    {
        _user = user;
        _id = UsableID.DAMAGE;

        _applyDuration = applyTime;
        _effectDuration = duration;
        _effectAmount = amount;
    }

    public override void Update(float deltaTime)
    {
        //Increment the apply timer
        _applyTimer += deltaTime;

        if (_applyTimer > _applyDuration)
        {
            //if not applied, then set it to true
            if (!_isApplied)
            {
                _isApplied = true;
                //Apply the damage buff stat to the player
                _user.GetComponent<PlayerController>().EquipStat(new StatValues(StatID.DAMAGE_BOOST, _effectAmount));
            }

            //Increment the effect timer
            _effectTimer += Time.deltaTime;

            if (_effectTimer > _effectDuration)
            {
                //Remove the damage buff from the player
                _user.GetComponent<PlayerController>().UnequipStat(new StatValues(StatID.DAMAGE_BOOST, _effectAmount));
                _user.GetComponent<PlayerUI>().RemoveUsable(this);
            }
        }
    }

    public override float GetApplyTimeRemaining()
    {
        return _applyDuration - _applyTimer;
    }
}
