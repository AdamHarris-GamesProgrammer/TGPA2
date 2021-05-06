using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICheckPlayerState : AIState
{
    LastKnownLocation _lastKnownLocation;

    


    public void Enter(AIAgent agent)
    {
        if(_lastKnownLocation == null)
        {
            _lastKnownLocation = GameObject.FindObjectOfType<LastKnownLocation>();
        }

        bool successful = false;
        Vector3 finalDestination = Vector3.zero;

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

            Debug.Log("Point: " + point);

            Vector3 destination = new Vector3(point.x * _lastKnownLocation.RadiusAroundPlayer / 2, agent.GetPlayer().position.y, point.y * _lastKnownLocation.RadiusAroundPlayer / 2);

            Debug.Log("Destination: " + destination);

            finalDestination = agent.GetPlayer().position + destination;

            Debug.Log("Final Destination: " + finalDestination);

            successful = Physics.Raycast(finalDestination, _lastKnownLocation.transform.position - finalDestination);

            Debug.Log("Successful: " + successful);

        } while (!successful);

        agent.GetComponent<NavMeshAgent>().SetDestination(finalDestination);
    }

    public void Exit(AIAgent agent)
    {

    }

    public void Update(AIAgent agent)
    {

    }

    public AiStateId GetID()
    {
        return AiStateId.GotToPlayerLocation;
    }
}
