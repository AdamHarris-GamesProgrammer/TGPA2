using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackPlayerState : AIState
{
    Health _playerHealth;

    public void Enter(AIAgent agent)
    {
        agent._aiWeapon.SetTarget(agent._player.transform);
        agent._agent.stoppingDistance = 5.0f;
        agent._aiWeapon.SetFiring(true);

        _playerHealth = agent._player.GetComponent<Health>();
    }

    public void Exit(AIAgent agent)
    {
        agent._agent.stoppingDistance = 1.0f;
        agent._aiWeapon.SetFiring(false);
        agent._aiWeapon.SetTarget(null);
    }

    public AiStateId GetID()
    {
        return AiStateId.AttackPlayer;
    }

    public void Update(AIAgent agent)
    {
        agent._agent.SetDestination(agent._player.transform.position);

        if (_playerHealth.IsDead())
        {
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }
    }

}
