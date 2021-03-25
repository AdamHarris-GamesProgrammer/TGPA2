using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    UIHealthBar _healthBar;
    Ragdoll _ragdoll;
    AIWeapons _aiWeapon;

    public AIDeathState(AIAgent agent)
    {
        _healthBar = agent.GetComponentInChildren<UIHealthBar>();
        _ragdoll = agent.GetComponent<Ragdoll>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
    }

    public void Enter(AIAgent agent)
    {
        _healthBar.gameObject.SetActive(false);
        _ragdoll.ActivateRagdoll();
        _aiWeapon.DropWeapon();
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
