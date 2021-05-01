using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Consumables/MedKit"))]
public class MedKit : ActionItem
{
    [SerializeField] float _healingAmount = 20;

    public override void Use(GameObject user)
    {
        user.GetComponent<Health>().Heal(_healingAmount);
    }
}
