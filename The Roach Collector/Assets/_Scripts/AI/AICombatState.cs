using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICombatState : AIState
{
    List<CombatSnippet> _combatBehaviours;
    CombatSnippet _currentSnippet;

    public AICombatState(AIAgent agent)
    {
        _combatBehaviours = new List<CombatSnippet>();

        RegisterSnippet(agent, new AdvanceSnippet());
        RegisterSnippet(agent, new CoverSnippet());
        RegisterSnippet(agent, new CallForBackupSnippet());
        RegisterSnippet(agent, new ReloadSnippet());
        RegisterSnippet(agent, new RetreatSnippet());
        RegisterSnippet(agent, new SetAlarmSnippet());
    }

    public void Update(AIAgent agent)
    {
        if (_currentSnippet.IsFinished())
        {
            int highestScore = 0;

            foreach (CombatSnippet behavior in _combatBehaviours)
            {
                int score = behavior.Evaluate(agent);

                //Checks which snippet is optimal 
                if (score > highestScore)
                {
                    highestScore = score;
                    _currentSnippet = behavior;
                }
            }
        }
        else
        {
            _currentSnippet.Action(agent);
        }
    }

    private void RegisterSnippet(AIAgent agent, CombatSnippet snippet)
    {
        snippet.Initialize(agent);
        _combatBehaviours.Add(snippet);
    }

    public AiStateId GetID()
    {
        return AiStateId.CombatState;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Entering Combat state");

        //Decide starting snippet
        int highestScore = 0;
        foreach (CombatSnippet behavior in _combatBehaviours)
        {
            int score = behavior.Evaluate(agent);

            //Checks which snippet is optimal 
            if (score > highestScore)
            {
                highestScore = score;
                _currentSnippet = behavior;
            }
        }
    }

    public void Exit(AIAgent agent)
    {
        Debug.Log("Exiting Combat state");
    }
}
