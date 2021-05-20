using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBossChase : AIState
{
    MeshSockets _sockets;
    //AIWeapons _aiWeapon;
    NavMeshAgent _navAgent;
    [SerializeField]
    Transform _player;

    BrickAgent _agent;

    float _timer = 0.0f;

    public AIBossChase(BrickAgent agent)
    {
        _sockets = agent.GetComponent<MeshSockets>();
        //_aiWeapon = agent.GetComponent<AIWeapons>();
        _navAgent = agent.GetComponent<NavMeshAgent>();
        _player = agent.GetPlayer();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = agent;
    }

    public void Enter()
    {
        //_sockets.Attach(_aiWeapon.transform, MeshSockets.SocketID.RightHand);

    }

    public void Exit()
    {

    }

    public AiStateId GetID()
    {
        return AiStateId.BossChase;
    }

    public void Update()
    {
        
        //Debug.Log("Boss STate Update");
        if (_player == null)
        {
            Debug.Log("player agent null2");
        }
        //If the agent is not enabled then we dont update
        if (!_navAgent.enabled) return;

        _timer -= Time.deltaTime;

        //Only sets the destination every few seconds and only if the player is further than a certain distance. This is a optimization as setting the destination is computationally expensive
        if (_timer < 0.0f)
        {
            float sqrDistance = (_player.position - _navAgent.destination).sqrMagnitude;
            if (sqrDistance > _agent._config._minDistance * _agent._config._minDistance)
            {
                _navAgent.destination = _player.position;
            }
            _timer = _agent._config._maxTime;
        }

        if (Vector3.Distance(_player.position, _navAgent.transform.position) < _agent._config._attackDistance)
        {
            _agent.stateMachine.ChangeState(AiStateId.BrickMelee);
        }
    }
}
