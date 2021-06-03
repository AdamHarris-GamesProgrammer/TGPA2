using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

[CreateAssetMenu(menuName=("Consumables/MedKit"))]
public class MedKitItem : ActionItem
{
    [SerializeField] float _healingAmount = 20;
    [SerializeField] float _timeToApply = 5.0f;
    [SerializeField] float _timeForFullEffect = 5.0f;

    public override void Use(GameObject user, int index)
    {
        //Adds this item to the players ui and instantiates the usable object
        user.GetComponent<PlayerUI>().AddUsable(new MedicalUsable(user, _timeToApply, _timeForFullEffect, _healingAmount));

    }
}
