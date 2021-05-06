using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeState : AIState
{

    Animator AnimController;

    public AIMeleeState(AIAgent agent)
    {
        AnimController = agent.GetComponent<Animator>();
    }

    public void Enter(AIAgent agent)
    {
        
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AiStateId GetID()
    {
        return AiStateId.Melee;
    }

    public void Update(AIAgent agent)
    {

        if(Vector3.Distance(agent.GetPlayer().position, agent.transform.position) < 3.0f)
        {
            AnimController.SetTrigger("Stab");

        }

        if (AnimController.GetCurrentAnimatorStateInfo(0).IsName("Stabbing") && AnimController.GetCurrentAnimatorStateInfo(0).length > AnimController.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            agent.GetComponent<WeaponStabCheck>().SetStabbing(true);
        }
        else
        {
            agent.GetComponent<WeaponStabCheck>().SetStabbing(false);
        }

    }

}
