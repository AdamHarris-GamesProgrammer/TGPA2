using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICombatState : AIState
{
    List<CombatSnippet> _combatBehaviours;
    CombatSnippet _currentSnippet;

    Health _playerHealth;

    public AICombatState(AIAgent agent)
    {
        _combatBehaviours = new List<CombatSnippet>();
        _playerHealth = agent.GetPlayer().GetComponent<Health>();

        RegisterSnippet(agent, new AdvanceSnippet());
        RegisterSnippet(agent, new CoverSnippet());
        RegisterSnippet(agent, new CallForBackupSnippet());
        RegisterSnippet(agent, new ReloadSnippet());
        RegisterSnippet(agent, new SetAlarmSnippet());
    }

    public void Update(AIAgent agent)
    {
        if (_playerHealth.IsDead)
        {
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }

        if (_currentSnippet.IsFinished())
        {
            int highestScore = 0;

            CombatSnippet newSnippet = null;

            foreach (CombatSnippet behavior in _combatBehaviours)
            {
                int score = behavior.Evaluate();

                //Checks which snippet is optimal 
                if (score > highestScore)
                {
                    highestScore = score;
                    newSnippet = behavior;
                }
            }

            SwitchSnippets(agent, newSnippet);
        }
        else
        {
            _currentSnippet.Action();
        }
    }

    private void SwitchSnippets(AIAgent agent, CombatSnippet newSnippet)
    {
        _currentSnippet = newSnippet;
        _currentSnippet.EnterSnippet();
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
        //Decide starting snippet
        int highestScore = 0;
        foreach (CombatSnippet behavior in _combatBehaviours)
        {
            int score = behavior.Evaluate();

            //Checks which snippet is optimal 
            if (score > highestScore)
            {
                highestScore = score;
                _currentSnippet = behavior;
            }
        }
        agent.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public void Exit(AIAgent agent)
    {

    }
}
