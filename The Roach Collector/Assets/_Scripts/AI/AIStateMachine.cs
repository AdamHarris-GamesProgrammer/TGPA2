using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine 
{
    public AIState[] states;
    public AIAgent _agent;
    public AiStateId _currentState;
    public AIStateMachine(AIAgent agent)
    {
        _agent = agent;
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        states = new AIState[numStates];
    }

    public void RegisterState(AIState state)
    {
        //Adds the state to the array
        int index = (int)state.GetID();
        states[index] = state;
    }

    public AIState GetState(AiStateId stateId)
    {
        int index = (int)stateId;

        AIState state = states[index];
        //if(state == null) Debug.Log(stateId + " has not been registered to " + _agent.name);

        return states[index];
    }

    public void Update()
    {
        GetState(_currentState)?.Update();
    }

    public void ChangeState(AiStateId newState)
    {
        //Debug.Log(newState);
        GetState(_currentState)?.Exit();
        _currentState = newState;
        GetState(_currentState)?.Enter();
    }
}
