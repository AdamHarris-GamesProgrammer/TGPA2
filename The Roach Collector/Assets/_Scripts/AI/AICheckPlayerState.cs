﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICheckPlayerState : AIState
{
    LastKnownLocation _lastKnownLocation;
    NavMeshAgent _navAgent;

    bool _arrivedAtPoint = false;
    AIAgent _agent;

    float _investigateDuration = 10.0f;
    float _investigateTimer = 0.0f;

    static AIAgent _selectedAI = null;
    public AIAgent SelectedAI { get { return _selectedAI; } set { _selectedAI = value; } }

    public AICheckPlayerState(AIAgent agent)
    {
        _agent = agent;
        _navAgent = agent.GetComponent<NavMeshAgent>();
    }


    public void Enter()
    {
        _arrivedAtPoint = false;

        if (_lastKnownLocation == null)
        {
            _lastKnownLocation = GameObject.FindObjectOfType<LastKnownLocation>();

            if (_lastKnownLocation == null)
            {
                Debug.LogError("Last Player Location prefab not placed in scene.");
            }

        }

        _navAgent.SetDestination(_lastKnownLocation.GeneratePointInRange(7.5f));
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (!_arrivedAtPoint && _navAgent.remainingDistance < 1.5f)
        {
            _arrivedAtPoint = true;


        }

        Vector3 direction = _lastKnownLocation.transform.position - _agent.transform.position;

        Quaternion look = Quaternion.Slerp(_agent.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime);

        _agent.transform.rotation = look;

        List<AIAgent> agents = _lastKnownLocation.GetEnemiesInRange(7.5f);

        if (_arrivedAtPoint)
        {
            //if there is no selected AI to investigate
            if (_selectedAI == null)
            {
                _investigateTimer += Time.deltaTime;

                //Timer check
                if (_investigateTimer > _investigateDuration)
                {
                    _selectedAI = _agent;
                }

                
                if (agents.Count > 3)
                {
                    if (_selectedAI == null)
                    {
                        _selectedAI = agents[(int)Random.Range(0, agents.Count)];
                    }
                }
            }
            else
            {
                if (_selectedAI == _agent)
                {
                    _navAgent.SetDestination(_lastKnownLocation.transform.position);

                    if(_navAgent.remainingDistance < 2.0f)
                    {
                        _selectedAI = null;
                    }
                }
            }

            if (_agent.GetComponent<FieldOfView>().IsEnemyInFOV)
            {
                foreach (AIAgent ally in agents)
                {
                    ally.stateMachine.ChangeState(AiStateId.CombatState);
                }
            }
            else
            {
                foreach (AIAgent ally in agents)
                {
                    ally.stateMachine.ChangeState(AiStateId.SearchForPlayer);
                }
            }

        }
    }


    public AiStateId GetID()
    {
        return AiStateId.GotToPlayerLocation;
    }
}
