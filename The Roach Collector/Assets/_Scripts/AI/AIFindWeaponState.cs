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
            agent.stateMachine.ChangeState(AiStateId.CombatState);
        }
    }


}
