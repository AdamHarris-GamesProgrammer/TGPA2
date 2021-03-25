using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindWeaponState : AIState
{
    public void Enter(AIAgent agent)
    {
        WeaponPickup pickup = FindClosestWeapon(agent);
        agent._agent.destination = pickup.gameObject.transform.position;
        agent._agent.speed = 5.0f;
    }

    public void Exit(AIAgent agent)
    {
    }

    public AiStateId GetID()
    {
        return AiStateId.FindWeapon;
    }

    public void Update(AIAgent agent)
    {
        if(agent._aiWeapon.HasWeapon())
        {
            agent.stateMachine.ChangeState(AiStateId.AttackPlayer);
        }
    }

    private WeaponPickup FindClosestWeapon(AIAgent agent)
    {
        WeaponPickup[] weapons = Object.FindObjectsOfType<WeaponPickup>();
        WeaponPickup closestWeapon = null;

        float closestDistance = float.MaxValue;
        foreach(var weapon in weapons)
        {
            float distanceToWeapon = Vector3.Distance(agent.transform.position, weapon.transform.position);
            if(distanceToWeapon < closestDistance)
            {
                closestDistance = distanceToWeapon;
                closestWeapon = weapon;
            }
        }

        return closestWeapon;
    }
}
