using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdvanceSnippet : CombatSnippet
{
    NavMeshAgent _navAgent;
    AIWeapons _aiWeapon;
    AIHealth _aiHealth;

    AIAgent _agent;

    float _timer = 0.0f;

    public void Action()
    {
        _timer += Time.deltaTime;

        Vector3 playerPos = _agent.GetPlayer().position;

        _navAgent.SetDestination(playerPos);

        //Set the player as the target
        _aiWeapon.SetTarget(_agent.GetPlayer());

        //Start firing
        _aiWeapon.SetFiring(true);
    }

    public void EnterSnippet()
    {
        //Debug.Log("Advance Snippet");

        _timer = 0.0f;

        //TODO: Change this so some AI will rush the player
        _navAgent.stoppingDistance = 10.0f;
    }

    public int Evaluate()
    {
        int returnScore = 0;

        float healthRatio = _aiHealth.HealthRatio;

        if (healthRatio > _agent._config._advanceEnterHealthRatio)
        {
            returnScore = 20;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _navAgent = agent.GetComponent<NavMeshAgent>();
        _aiHealth = agent.GetComponent<AIHealth>();
        _agent = agent;
    }

    public bool IsFinished()
    {
        //Checks if the enemy is low on health or if the state duration is up
        return (_aiHealth.HealthRatio < 0.5f || _timer >= _agent._config._advanceStateDuration);

    }
}
