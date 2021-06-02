using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickAIIdleState : AIState
{
    AIAgent _agent;
    public BrickAIIdleState(AIAgent agent)
    {

    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public AiStateId GetID()
    {
        return AiStateId.BrickIdle;
    }

    void AIState.Update()
    {

    }
}
