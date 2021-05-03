using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIIdleState : AIState
{
    Transform _player;
    FieldOfView _FOV;

    public AIIdleState(AIAgent agent)
    {
        _player = agent.GetPlayer();
        _FOV = agent.GetComponent<FieldOfView>();
    }

    public void Enter(AIAgent agent)
    {
        agent.GetComponent<AIWeapons>().SetTarget(null);
        agent.GetComponent<AIWeapons>().SetFiring(false);
        agent.GetComponent<NavMeshAgent>().isStopped = true;

    }

    public void Exit(AIAgent agent)
    {
    }

    public AiStateId GetID()
    {
        return AiStateId.Idle;
    }

    public void Update(AIAgent agent)
    {
        //Check if player is in the AI's field of view
        if(_FOV.IsEnemyInFOV())
        {
            //Player is in view, change to chase state
            Debug.Log("Player in FOV");
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }


    }

}
