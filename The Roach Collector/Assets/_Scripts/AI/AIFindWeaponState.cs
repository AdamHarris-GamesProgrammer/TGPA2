using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFindWeaponState : AIState
{
    AIAgent _agent;
    AIWeapons _aiWeapon;

    public AIFindWeaponState(AIAgent agent)
    {
        _agent = agent;
        _aiWeapon = agent.GetComponent<AIWeapons>();
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public AiStateId GetID()
    {
        return AiStateId.FindWeapon;
    }

    public void Update()
    {
        if(_aiWeapon.HasWeapon())
        {
            _agent.stateMachine.ChangeState(AiStateId.CombatState);
        }
    }


}
