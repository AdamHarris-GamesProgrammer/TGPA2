using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIChasePlayerState : AIState
{
    

    float _timer = 0.0f;

    public void Enter(AIAgent agent)
    {
        agent._sockets.Attach(agent._aiWeapon.transform, MeshSockets.SocketID.RightHand);
    }

    public void Exit(AIAgent agent)
    {

    }

    public AiStateId GetID()
    {
        return AiStateId.ChasePlayer;
    }

    public void Update(AIAgent agent)
    {
        //If the agent is not enabled then we dont update
        if (!agent._agent.enabled) return;

        _timer -= Time.deltaTime;

        //Only sets the destination every few seconds and only if the player is further than a certain distance. This is a optimization as setting the destination is computationally expensive
        if (_timer < 0.0f)
        {
            float sqrDistance = (agent._player.position - agent._agent.destination).sqrMagnitude;
            if (sqrDistance > agent._config._minDistance * agent._config._minDistance)
            {
                agent._agent.destination = agent._player.position;
            }
            _timer = agent._config._maxTime;
        }
    }
}
