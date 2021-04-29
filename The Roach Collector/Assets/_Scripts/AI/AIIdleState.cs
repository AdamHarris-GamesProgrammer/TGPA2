using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    Transform _player;


    public AIIdleState(AIAgent agent)
    {
        _player = agent.GetPlayer();
    }

    public void Enter(AIAgent agent)
    {
    }

    public void Exit(AIAgent agent)
    {
    }

    public AiStateId GetID()
    {
        return AiStateId.Idle;
    }

    public void Update(AIAgent agent)
    {
        Vector3 playerDirection = _player.position - agent.transform.position;
        if (playerDirection.sqrMagnitude > agent._config._maxSightDistance * agent._config._maxSightDistance)
        {
            //Player is too far away
            return;
        }

        Vector3 agentDirection = agent.transform.forward;

        playerDirection.Normalize();

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if(dotProduct > 0.0f)
        {
            //TODO Change this to a proper detection system
            agent.stateMachine.ChangeState(AiStateId.CombatState);
        }

    }

}
