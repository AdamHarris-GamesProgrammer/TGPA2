using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFindWeaponState : AIState
{
    NavMeshAgent _agent;
    AIWeapons _aiWeapon;

    public AIFindWeaponState(AIAgent agent)
    {
        _agent = agent.GetComponent<NavMeshAgent>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
    }

    public void Enter(AIAgent agent)
    {
        WeaponPickup pickup = FindClosestWeapon(agent);
        _agent.destination = pickup.gameObject.transform.position;
        _agent.speed = 5.0f;
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
        if(_aiWeapon.HasWeapon())
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
