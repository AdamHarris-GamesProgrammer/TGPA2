using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public void Enter(AIAgent agent)
    {
        agent._healthBar.gameObject.SetActive(false);
        agent._ragdoll.ActivateRagdoll();
        agent._aiWeapon.DropWeapon();
    }

    public void Exit(AIAgent agent)
    {
    }

    public AiStateId GetID()
    {
        return AiStateId.Death;
    }

    public void Update(AIAgent agent)
    {
    }
}
