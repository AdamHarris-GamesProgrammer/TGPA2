using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrickMelee : AIState
{
    Health _playerHealth;
    NavMeshAgent _navAgent;
    Transform _player;
    Animator thisAnim;
    private BrickAgent _agent;
    FieldOfView _FOV;

    public BrickMelee(BrickAgent agent)
    {
        _player = agent.GetPlayer();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _navAgent = agent.GetComponent<NavMeshAgent>();
        _playerHealth = agent.GetPlayer().GetComponent<Health>();
        thisAnim = thisAnim.GetComponent<Animator>();
        _agent = agent;
        _FOV = _agent.GetComponent<FieldOfView>();
        
    }

    public void Enter()
    {
        Debug.Log("Entered Melee state");
        //_agent.destination = _player.transform.position;
    }

    public void Exit()
    {
        
    }

    public AiStateId GetID()
    {
        return AiStateId.BrickMelee;
    }

    public void Update()
    {
        Debug.Log("Entered Melee state");
        if (Vector3.Distance(_player.position, _navAgent.transform.position) < _agent._config._attackDistance)
        {
            _agent.stateMachine.ChangeState(AiStateId.CombatState);
        }
        if (_playerHealth.IsDead)
        {
            _agent.stateMachine.ChangeState(AiStateId.Idle);
        }
        
    }

    
   
}
