using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIIdleState : AIState
{
    Transform _player;
    FieldOfView _FOV;
    SoundPerception _soundPerception;

    public AIIdleState(AIAgent agent)
    {
        _player = agent.GetPlayer();
        _FOV = agent.GetComponent<FieldOfView>();
        _soundPerception = agent.GetComponentInChildren<SoundPerception>();
    }

    public void Enter(AIAgent agent)
    {
        agent.GetComponent<AIWeapons>().SetTarget(null);
        agent.GetComponent<AIWeapons>().SetFiring(false);
        agent.GetComponent<NavMeshAgent>().isStopped = true;

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
            //TODO: Incorporate Perception system 
            //TODO: Add in Object for players last known position
            //TODO: Check out if the player is still there. 
            agent.stateMachine.ChangeState(AiStateId.GotToPlayerLocation);
        }


    }

}
