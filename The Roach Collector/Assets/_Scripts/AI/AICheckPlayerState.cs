﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICheckPlayerState : AIState
{
    LastKnownLocation _lastKnownLocation;
    NavMeshAgent _navAgent;

    bool _arrivedAtPoint = false;

    public AICheckPlayerState(AIAgent agent)
    {
        _navAgent = agent.GetComponent<NavMeshAgent>();
    }


    public void Enter(AIAgent agent)
    {
        _arrivedAtPoint = false;

        if(_lastKnownLocation == null)
        {
            _lastKnownLocation = GameObject.FindObjectOfType<LastKnownLocation>();
        }

        bool successful = false;
        Vector3 finalDestination = Vector3.zero;

        //Debug.Log("check player state");

        do
        {
            Vector2 point = Random.insideUnitCircle;
            float x = point.x;
            float y = point.y;

            x *= 2.0f;
            y *= 2.0f;

            x -= 1.0f;
            y -= 1.0f;

            point.x = x;
            point.y = y;

            //Debug.Log("Point: " + point);

            Vector3 destination = new Vector3(point.x * _lastKnownLocation.RadiusAroundPlayer / 2, agent.GetPlayer().position.y, point.y * _lastKnownLocation.RadiusAroundPlayer / 2);

            //Debug.Log("Destination: " + destination);

            finalDestination = agent.GetPlayer().position + destination;

            //Debug.Log("Final Destination: " + finalDestination);

            successful = Physics.Raycast(finalDestination, _lastKnownLocation.transform.position - finalDestination);

            //Debug.Log("Successful: " + successful);

        } while (!successful);

        _navAgent.SetDestination(finalDestination);
    }

    public void Exit(AIAgent agent)
    {

    }

    public void Update(AIAgent agent)
    {
        if(!_arrivedAtPoint && _navAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            _arrivedAtPoint = true;

            Vector3 direction = _lastKnownLocation.transform.position - agent.transform.position;

            Quaternion look = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime);

            agent.transform.rotation = look;
        }

        //TODO: Wait for timer saying they should go over to the player
        //Or if there are sufficient allies in the area 

        //Then select a single AI to go over and see if the player is there
            //If the agent is there and the player is then actrivate attack mode
            //if the agent is there and the player has gone then activate search mode.
                
    }


    public AiStateId GetID()
    {
        return AiStateId.GotToPlayerLocation;
    }
}
