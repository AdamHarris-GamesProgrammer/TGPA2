using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMeleeState : AIState
{

    Animator _animController;
    AIAgent _agent;
    NavMeshAgent _navAgent;
    WeaponStabCheck _stabCheck;

    bool _isStabbing;

    public AIMeleeState(AIAgent agent)
    {
        _agent = agent;
        _animController = _agent.GetComponent<Animator>();
        _navAgent = _agent.GetComponent<NavMeshAgent>();
        _stabCheck = _agent.GetComponent<WeaponStabCheck>();
    }

    public void Enter() {}

    public void Exit() {}

    public AiStateId GetID()
    {
        return AiStateId.Melee;
    }

    public void Update()
    {
        //Makes the AI chase the player
        _navAgent.SetDestination(_agent.Player.position);

        //if the distance between the player and the agent is less than 1.5f
        if(_navAgent.remainingDistance < 1.5f)
        {
            //Set Stab Trigger
            _animController.SetTrigger("Stab");
        }

        if (_stabCheck.GetStabbing()) _navAgent.isStopped = true;
        else _navAgent.isStopped = false;
    }

}
