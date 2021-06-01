using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

[CreateAssetMenu(menuName = "Consumables/Damage Buff Item")]
public class DamageBuffItem : ActionItem
{
    [Header("Damage Buff Item Settings")]
    [SerializeField] float _damageBuffAmount = 20.0f;
    [SerializeField] float _timeToApply = 2.0f;
    [SerializeField] float _duration = 10.0f;

    public override void Use(GameObject user, int index)
    {
        //Adds this item to the players ui and instantiates the usable object
        user.GetComponent<PlayerUI>().AddUsable(new DamageResistanceUsable(user, _timeToApply, _duration, _damageBuffAmount));
    }
}
