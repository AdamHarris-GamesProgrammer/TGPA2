using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
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
        Vector3 playerDirection = agent._player.position - agent.transform.position;
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
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }

    }

}
