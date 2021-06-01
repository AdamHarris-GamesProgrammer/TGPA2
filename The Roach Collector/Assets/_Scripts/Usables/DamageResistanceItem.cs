using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

[CreateAssetMenu(menuName ="Consumables/Damage Resistance Item")]
public class DamageResistanceItem : ActionItem
{
    [Header("Damage Resistance Item Settings")]
    [SerializeField] float _damageResistanceAmount = 20.0f;
    [SerializeField] float _timeToApply = 2.0f;
    [SerializeField] float _duration = 10.0f;

    public override void Use(GameObject user, int index)
    {
        user.GetComponent<PlayerUI>().AddUsable(new DamageResistanceUsable(user, _timeToApply, _duration, _damageResistanceAmount));
    }
}
