using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIIdleState : AIState
{
    FieldOfView _FOV;
    SoundPerception _soundPerception;
    Vector3 _defaultPosition;
    AIAgent _agent;

    public AIIdleState(AIAgent agent)
    {
        _agent = agent;
        _FOV = agent.GetComponent<FieldOfView>();
        _soundPerception = agent.GetComponentInChildren<SoundPerception>();
        _defaultPosition = agent.transform.position;
    }

    public void Enter()
    {
        _agent.GetComponent<AIWeapons>().SetTarget(null);
        _agent.GetComponent<AIWeapons>().SetFiring(false);
        _agent.GetComponent<NavMeshAgent>().SetDestination(_defaultPosition);
    }

    public void Exit() {}

    public AiStateId GetID()
    {
        return AiStateId.Idle;
    }

    public void Update()
    {
        //Check if player is in the AI's field of view
        if(_FOV.IsEnemyInFOV || _soundPerception.IsHeard)
        {
            //Player is in view, change to chase state
            //Debug.Log("Player in FOV");
            _agent.stateMachine.ChangeState(AiStateId.GotToPlayerLocation);
        }


    }

}
