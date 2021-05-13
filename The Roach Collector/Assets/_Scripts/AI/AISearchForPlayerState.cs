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


    Vector3[] _searchLocations = new Vector3[3];

    public AISearchForPlayerState(AIAgent agent)
    {
        _playersKnownLocation = GameObject.FindObjectOfType<LastKnownLocation>();

        if(_playersKnownLocation == null)
        {
            Debug.LogError("Last Player Location prefab not placed in scene.");
        }

        _navAgent = agent.GetComponent<NavMeshAgent>();

        _fov = agent.GetComponent<FieldOfView>();
        
    } 

    public void Enter(AIAgent agent)
    {
        _index = 0;

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

        _navAgent.SetDestination(_searchLocations[_index]);
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
        if (_fov.IsEnemyInFOV)
        {
            agent.stateMachine.ChangeState(AiStateId.CombatState);
        }


        if(_navAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            _timer += Time.deltaTime;

            if(_timer > _timeAtEachSearchPoint)
            {
                _timer = 0.0f;
                _index++;

                if(_index < _searchLocations.Length)
                {
                    _navAgent.SetDestination(_searchLocations[_index]);
                    //Debug.Log("Next Search point");
                }
                else
                {
                    GameObject.FindObjectOfType<PlayerController>().IsDetected = false;
                    agent.stateMachine.ChangeState(AiStateId.Idle);
                    //Debug.Log("End Search");
                }
            }


        }
    }
}
