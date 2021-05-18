using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIIdleState : AIState
{
    FieldOfView _FOV;
    SoundPerception _soundPerception;
    Vector3 _defaultPosition;

    public AIIdleState(AIAgent agent)
    {
        _FOV = agent.GetComponent<FieldOfView>();
        _soundPerception = agent.GetComponentInChildren<SoundPerception>();
        _defaultPosition = agent.transform.position;
    }

    public void Enter(AIAgent agent)
    {
        agent.GetComponent<AIWeapons>().SetTarget(null);
        agent.GetComponent<AIWeapons>().SetFiring(false);
        agent.GetComponent<NavMeshAgent>().SetDestination(_defaultPosition);
    }

    public void Exit(AIAgent agent)
    {
        agent.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public AiStateId GetID()
    {
        return AiStateId.Idle;
    }

    public void Update(AIAgent agent)
    {
        //Check if player is in the AI's field of view
        if(_FOV.IsEnemyInFOV || _soundPerception.IsHeard)
        {
            //Player is in view, change to chase state
            //Debug.Log("Player in FOV");
            agent.stateMachine.ChangeState(AiStateId.GotToPlayerLocation);
        }


    }

}
