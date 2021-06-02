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
    int _index = 0;

    FieldOfView _fov;
    AIAgent _agent;

    Vector3[] _searchLocations = new Vector3[3];

    public AISearchForPlayerState(AIAgent agent)
    {
        _agent = agent;
        _playersKnownLocation = GameObject.FindObjectOfType<LastKnownLocation>();

        if(_playersKnownLocation == null) Debug.LogError("Last Player Location prefab not placed in scene.");

        _navAgent = agent.GetComponent<NavMeshAgent>();

        _fov = agent.GetComponent<FieldOfView>();
        
    } 

    public void Enter()
    {
        _index = 0;

        //Fill each search position
        for (int i = 0; i < 3; i++)
        {
            bool successful = false;

            //Loop until a suitable position is found
            do
            {
                //Generates a position
                Vector3 pos = Random.insideUnitSphere * 25.0f;
                pos = _playersKnownLocation.transform.position + pos;
                pos.y = _playersKnownLocation.transform.position.y;

                //Sample the position
                NavMeshHit hit;
                successful = NavMesh.SamplePosition(pos, out hit, 25.0f, NavMesh.AllAreas);

                if (successful) _searchLocations[i] = pos;

            } while (!successful);
        }

        //Sends the agent to the first search location
        _navAgent.SetDestination(_searchLocations[0]);
    }

    public void Exit() {}

    public AiStateId GetID() { return AiStateId.SearchForPlayer; }

    public void Update()
    {
        //If the player is in view then switch to combat 
        if (_fov.IsEnemyInFOV) _agent.Aggrevate();


        if(_navAgent.remainingDistance <= 1.5f)
        {
            _timer += Time.deltaTime;

            //If the AI have stood still for enough time
            if(_timer > _timeAtEachSearchPoint)
            {
                //Go to next point
                _timer = 0.0f;
                _index++;

                //Sets the next position to go to
                if(_index < _searchLocations.Length)
                {
                    _navAgent.SetDestination(_searchLocations[_index]);
                }
                //Return to their default state
                else
                {
                    GameObject.FindObjectOfType<PlayerController>().IsDetected = false;
                    _agent.ReturnToDefaultState();
                }
            }


        }
    }
}
