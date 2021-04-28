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
        int index = (int)state.GetID();
        states[index] = state;

    }

    public AIState GetState(AiStateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }

    public void Update()
    {
        GetState(_currentState)?.Update(_agent);
    }

    public void ChangeState(AiStateId newState)
    {
        GetState(_currentState)?.Exit(_agent);
        _currentState = newState;
        GetState(_currentState)?.Enter(_agent);
    }
}
