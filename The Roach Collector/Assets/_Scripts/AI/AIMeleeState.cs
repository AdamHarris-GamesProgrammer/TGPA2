using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeState : AIState
{

    Animator AnimController;
    AIAgent _agent;

    public AIMeleeState(AIAgent agent)
    {
        AnimController = agent.GetComponent<Animator>();
        _agent = agent;
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

        if(Vector3.Distance(_agent.GetPlayer().position, _agent.transform.position) < 3.0f)
        {
            AnimController.SetTrigger("Stab");

        }

        if (AnimController.GetCurrentAnimatorStateInfo(0).IsName("Stabbing") && AnimController.GetCurrentAnimatorStateInfo(0).length > AnimController.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            _agent.GetComponent<WeaponStabCheck>().SetStabbing(true);
        }
        else
        {
            _agent.GetComponent<WeaponStabCheck>().SetStabbing(false);
        }

    }

}
