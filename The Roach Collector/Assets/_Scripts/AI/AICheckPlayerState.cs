using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICheckPlayerState : AIState
{
    static LastKnownLocation _lastKnownLocation;
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

            if (_lastKnownLocation == null) Debug.LogError("Last Player Location prefab not placed in scene.");

        }

        //Generates a point near the last known location of the player for the AI to stand
        _navAgent.SetDestination(_lastKnownLocation.GeneratePointInRange(7.5f));
    }

    public void Exit() {}

    public void Update()
    {
        //See if we have arrived at the point
        if (!_arrivedAtPoint && _navAgent.remainingDistance < 1.5f) _arrivedAtPoint = true;

        //Look at the last known location
        _agent.LookAtLastKnownLocation();

        //Gets the agents in the nearby area
        List<AIAgent> agents = _lastKnownLocation.GetEnemiesInRange(7.5f);

        if (_arrivedAtPoint)
        {
            //if there is no selected AI to investigate
            if (_selectedAI == null)
            {
                _investigateTimer += Time.deltaTime;

                //Timer check, basically waits for more "allies" to show up before choosing someone to go see the player
                if (_investigateTimer > _investigateDuration) _selectedAI = _agent;

                //if there are enough allies in the area, then forget the timer and go straight in
                if (agents.Count > 3)
                {
                    if (_selectedAI == null) _selectedAI = agents[(int)Random.Range(0, agents.Count)];
                }
            }
            else
            {
                //if this ai is the selected ai
                if (_selectedAI == _agent)
                {
                    //Set the destination
                    _navAgent.SetDestination(_lastKnownLocation.transform.position);

                    //When we are close enough set searched to true and change to the search state
                    if (_navAgent.remainingDistance < 2.0f) 
                    {
                        _selectedAI = null;
                        agents.ForEach(ally => ally.stateMachine.ChangeState(AiStateId.SearchForPlayer));
                    }
                }
            }
        }

        //Sees if the player is in the agents FOV, if so then switch all allies to combat state
        if (_agent.GetComponent<FieldOfView>().IsEnemyInFOV)
            agents.ForEach(ally => ally.Aggrevate());
    }


    public AiStateId GetID()
    {
        return AiStateId.GotToPlayerLocation;
    }
}
