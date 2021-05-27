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

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public AiStateId GetID()
    {
        return AiStateId.Melee;
    }

    public void Update()
    {
        //Makes the AI chase the player
        _navAgent.SetDestination(_agent.GetPlayer().position);


        //if the distance between the player and the agent is less than 1.5f
        if(Vector3.Distance(_agent.GetPlayer().position, _agent.transform.position) < 1.5f)
        {
            //Set Stab Trigger
            _animController.SetTrigger("Stab");


            //Checks if the stabbing animation is currently playing
            if (_animController.GetCurrentAnimatorStateInfo(0).IsName("Stabbing")
                && _animController.GetCurrentAnimatorStateInfo(0).length > _animController.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                _agent.GetComponent<WeaponStabCheck>().SetStabbing(true);
            }
            else
            {
                _agent.GetComponent<WeaponStabCheck>().SetStabbing(false);
            }
        }


    }

}
