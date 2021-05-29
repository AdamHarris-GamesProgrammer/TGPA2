using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMeleeState : AIState
{

    Animator _animController;
    AIAgent _agent;
    NavMeshAgent _navAgent;

    public AIMeleeState(AIAgent agent)
    {
        _agent = agent;
        _animController = _agent.GetComponent<Animator>();
        _navAgent = _agent.GetComponent<NavMeshAgent>();
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
        _navAgent.SetDestination(_agent.GetPlayer().position);


        //if the distance between the player and the agent is less than 1.5f
        if(_navAgent.remainingDistance < 1.5f)
        {
            //Set Stab Trigger
            _animController.SetTrigger("Stab");


            //Checks if the stabbing animation is currently playing
            AnimatorStateInfo state = _animController.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Stabbing") && state.length > state.normalizedTime) _agent.GetComponent<WeaponStabCheck>().SetStabbing(true);
            else _agent.GetComponent<WeaponStabCheck>().SetStabbing(false);
        }
    }

}
