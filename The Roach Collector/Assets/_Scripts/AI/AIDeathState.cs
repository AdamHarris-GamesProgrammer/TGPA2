using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDeathState : AIState
{
    UIHealthBar _healthBar;
    Ragdoll _ragdoll;
    AIWeapons _aiWeapon;

    NavMeshAgent _navAgent;

    public AIDeathState(AIAgent agent)
    {
        _healthBar = agent.GetComponentInChildren<UIHealthBar>();
        _ragdoll = agent.GetComponent<Ragdoll>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _navAgent = agent.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        _healthBar.gameObject.SetActive(false);
        _ragdoll.ActivateRagdoll();
        _aiWeapon.DropWeapon();

        //Stops the AI from moving to it's target
        _navAgent.isStopped = true;
        
    }

    public void Exit()
    {

    }

    public AiStateId GetID()
    {
        return AiStateId.Death;
    }

    public void Update()
    {
    }
}
