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
    private AIAgent _agent;
    FieldOfView _FOV;

    public BrickMelee(AIAgent agent)
    {
        _player = agent.GetPlayer();
        _navAgent = agent.GetComponent<NavMeshAgent>();
        _playerHealth = agent.GetPlayer().GetComponent<Health>();
        thisAnim = thisAnim.GetComponent<Animator>();
        _agent = agent;
        _FOV = _agent.GetComponent<FieldOfView>();
        
    }

    public void Enter()
    {

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
        if (_FOV.IsEnemyInFOV)
        {
            Debug.Log("Player detected");
            thisAnim.SetTrigger("isDetected");

        }
        if (!_navAgent.enabled)
        {
            return;
        }
        if (_playerHealth.IsDead)
        {
            _agent.stateMachine.ChangeState(AiStateId.Idle);
        }
        
    }

    
   
}
