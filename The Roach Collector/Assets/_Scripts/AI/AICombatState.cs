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

    AIAgent _agent;

    float _snippetDuration = 10.0f;
    float _snippetTimer = 0.0f;

    public AICombatState(AIAgent agent)
    {
        _agent = agent;
        _combatBehaviours = new List<CombatSnippet>();
        _playerHealth = _agent.GetPlayer().GetComponent<Health>();

        RegisterSnippet(new AdvanceSnippet());
        RegisterSnippet(new CoverSnippet());
        RegisterSnippet(new CallForBackupSnippet());
        RegisterSnippet(new ReloadSnippet());
        RegisterSnippet(new SetAlarmSnippet());
    }

    public void Update()
    {
        if (_playerHealth.IsDead)
        {
            Debug.Log("Player Dead");
            _agent.stateMachine.ChangeState(AiStateId.Idle);
        }

        if (_agent.BeingKilled) return;

        if(_currentSnippet == null) return;

        //Used to sample for better action every 10 seconds.
        _snippetTimer += Time.deltaTime;

        if (_currentSnippet.IsFinished() || _snippetTimer > _snippetDuration)
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

            SwitchSnippets(newSnippet);

            _snippetTimer = 0.0f;
        }
        else
        {
            _currentSnippet.Action();
        }
    }

    private void SwitchSnippets(CombatSnippet newSnippet)
    {
        _currentSnippet = newSnippet;
        _currentSnippet.EnterSnippet();
    }

    private void RegisterSnippet(CombatSnippet snippet)
    {
        snippet.Initialize(_agent);
        _combatBehaviours.Add(snippet);
    }

    public AiStateId GetID()
    {
        return AiStateId.CombatState;
    }

    public void Enter()
    {
        //Debug.Log("Entered Combat State");

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

        _currentSnippet.EnterSnippet();

        _agent.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public void Exit()
    {
        //Debug.Log("Exiting Combat State");
    }
}
