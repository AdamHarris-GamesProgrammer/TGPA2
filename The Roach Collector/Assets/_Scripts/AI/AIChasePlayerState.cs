using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIChasePlayerState : AIState
{
    MeshSockets _sockets;
    AIWeapons _aiWeapon;
    NavMeshAgent _agent;
    Transform _player;

    float _timer = 0.0f;

    public AIChasePlayerState(AIAgent agent)
    {
        _sockets = agent.GetComponent<MeshSockets>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _agent = agent.GetComponent<NavMeshAgent>();
        _player = agent.GetPlayer();

    }

    public void Enter(AIAgent agent)
    {
        _sockets.Attach(_aiWeapon.transform, MeshSockets.SocketID.RightHand);
    }

    public void Exit(AIAgent agent)
    {

    }

    public AiStateId GetID()
    {
        return AiStateId.ChasePlayer;
    }

    public void Update(AIAgent agent)
    {
        //If the agent is not enabled then we dont update
        if (!_agent.enabled) return;

        _timer -= Time.deltaTime;

        //Only sets the destination every few seconds and only if the player is further than a certain distance. This is a optimization as setting the destination is computationally expensive
        if (_timer < 0.0f)
        {
            float sqrDistance = (_player.position - _agent.destination).sqrMagnitude;
            if (sqrDistance > agent._config._minDistance * agent._config._minDistance)
            {
                _agent.destination = _player.position;
            }
            _timer = agent._config._maxTime;
        }

        if(Vector3.Distance(_player.position, _agent.transform.position) < agent._config._attackDistance)
        {
            agent.stateMachine.ChangeState(AiStateId.AttackPlayer);
        }
    }
}
