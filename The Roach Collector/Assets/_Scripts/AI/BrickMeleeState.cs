using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrickMelee : AIState
{
    Health _playerHealth;
    NavMeshAgent _agent;
    Transform _player;
    Animator thisAnim;

    public BrickMelee(AIAgent agent)
    {
        _player = agent.GetPlayer();
        _agent = _agent.GetComponent<NavMeshAgent>();
        _playerHealth = agent.GetPlayer().GetComponent<Health>();
        thisAnim = thisAnim.GetComponent<Animator>();
    }

    public void Enter(AIAgent agent)
    {

        //_agent.destination = _player.transform.position;
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AiStateId GetID()
    {
        return AiStateId.BrickMelee;
    }

    public void Update(AIAgent agent)
    {
        if (agent._FOV.IsEnemyInFOV())
        {
            Debug.Log("Player detected");
            thisAnim.SetTrigger("isDetected");

        }
        if (!agent.enabled)
        {
            return;
        }
        if (_playerHealth.IsDead())
        {
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }
        
    }

    
   
}
