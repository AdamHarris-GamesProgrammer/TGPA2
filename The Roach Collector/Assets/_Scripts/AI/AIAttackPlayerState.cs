using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackPlayerState : AIState
{
    Health _playerHealth;

    AIWeapons _aiWeapon;
    NavMeshAgent _agent;
    Transform _player;

    public AIAttackPlayerState(AIAgent agent)
    {
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _agent = agent.GetComponent<NavMeshAgent>();
        _player = agent.GetPlayer();
        _playerHealth = _player.GetComponent<Health>();
    }

    public void Enter(AIAgent agent)
    {
       _aiWeapon.SetTarget(_player);
       _agent.stoppingDistance = 5.0f;
       _aiWeapon.SetFiring(true);
    }

    public void Exit(AIAgent agent)
    {
        _agent.stoppingDistance = 1.0f;
        _aiWeapon.SetFiring(false);
        _aiWeapon.SetTarget(null);
    }

    public AiStateId GetID()
    {
        return AiStateId.AttackPlayer;
    }

    public void Update(AIAgent agent)
    {
        _agent.SetDestination(_player.position);

        if (_playerHealth.IsDead())
        {
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }
    }

}
