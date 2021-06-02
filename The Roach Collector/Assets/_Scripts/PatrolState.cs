using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : AIState
{
    AIAgent _agent;
    PatrolRoute _route;
    NavMeshAgent _navAgent;

    FieldOfView _fov;
    SoundPerception _soundPerception;

    int _index = 0;

    float _waitDuration;
    float _waitTimer = 0.0f;

    public PatrolState(AIAgent agent, PatrolRoute route, float movementSpeed, float waitDuration)
    {
        _agent = agent;
        _route = route;
        _navAgent = _agent.GetComponent<NavMeshAgent>();
        _fov = _agent.GetComponent<FieldOfView>();
        _soundPerception = _agent.GetComponentInChildren<SoundPerception>();

        _waitDuration = waitDuration;
        _navAgent.speed = movementSpeed;
    }

    public void Enter()
    {
        _index = _route.CycleIndex(_index);
        _navAgent.SetDestination(_route.GetNextPoint(_index));
        _navAgent.stoppingDistance = 0.5f;
    }

    public void Exit()
    {
        _navAgent.speed = _agent.DefaultMoveSpeed;
    }

    public AiStateId GetID()
    {
        return AiStateId.Patrol;
    }

    public void Update()
    {
        //if the distance to the point is small
        if(_navAgent.remainingDistance <= 1.5f)
        {
            //Start the wait timer
            _waitTimer += Time.deltaTime;
            if(_waitTimer > _waitDuration)
            {
                //Reset the timer
                _waitTimer = 0.0f;

                //Move to the next index
                _index = _route.CycleIndex(_index);
                _navAgent.SetDestination(_route.GetNextPoint(_index));
            }
        }

        //If player is seen or heard change to go to player location state
        if(_fov.IsEnemyInFOV || _soundPerception.IsHeard)
        {
            _agent.stateMachine.ChangeState(AiStateId.GotToPlayerLocation);
        }
    }
}
