using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.AI;

public class AISearchForPlayerState : AIState
{
    LastKnownLocation _playersKnownLocation;

    float _timeAtEachSearchPoint = 5.0f;
    float _timer = 0.0f;
    NavMeshAgent _navAgent;
    int index = 0;

    Vector3[] _searchLocations = new Vector3[3];

    public AISearchForPlayerState(AIAgent agent)
    {
        _playersKnownLocation = GameObject.FindObjectOfType<LastKnownLocation>();
        _navAgent = agent.GetComponent<NavMeshAgent>();


        
    } 

    public void Enter(AIAgent agent)
    {
        index = 0;

        for (int i = 0; i < 3; i++)
        {
            bool success = false;

            do
            {
                Vector3 pos = Random.insideUnitSphere * 25.0f;
                pos.y = _playersKnownLocation.transform.position.y;

                NavMeshHit hit;
                success = NavMesh.SamplePosition(pos, out hit, 25.0f, NavMesh.AllAreas);

                if (success)
                {
                    _searchLocations[i] = pos;
                }

            } while (!success);

            
        }

        _navAgent.SetDestination(_searchLocations[index]);
    }

    public void Exit(AIAgent agent)
    {

    }

    public AiStateId GetID()
    {
        return AiStateId.SearchForPlayer;
    }

    public void Update(AIAgent agent)
    {
        if(_navAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            _timer += Time.deltaTime;

            if(_timer > _timeAtEachSearchPoint)
            {
                _timer = 0.0f;
                index++;

                if(index < _searchLocations.Length)
                {
                    _navAgent.SetDestination(_searchLocations[index]);
                }
                else
                {
                    GameObject.FindObjectOfType<PlayerController>().IsDetected = false;
                    agent.stateMachine.ChangeState(AiStateId.Idle);
                }
            }


        }
    }
}
