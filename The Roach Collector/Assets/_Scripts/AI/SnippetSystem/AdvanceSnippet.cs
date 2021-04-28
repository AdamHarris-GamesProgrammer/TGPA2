using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdvanceSnippet : CombatSnippet
{
    NavMeshAgent _agent;
    AIWeapons _aiWeapon;
    AIHealth _aiHealth;

    float _duration = 1.5f;
    float _timer = 0.0f;

    public void Action(AIAgent agent)
    {
        _timer += Time.deltaTime;

        Vector3 playerPos = agent.GetPlayer().position;

        _agent.stoppingDistance = 10.0f;
        _agent.SetDestination(playerPos);

        //Set the player as the target
        _aiWeapon.SetTarget(agent.GetPlayer());

        if (_agent.remainingDistance < 1.5f)
        {
            _aiWeapon.SetFiring(true);
        }
        else
        {
            _aiWeapon.SetFiring(false);
        }
    }

    public void EnterSnippet(AIAgent agent)
    {
        //Debug.Log("Advance Snippet");

        _timer = 0.0f;

        _agent.stoppingDistance = 10.0f;
    }

    public int Evaluate(AIAgent agent)
    {
        int returnScore = 0;

        float healthRatio = _aiHealth.GetHealthRatio();

        if (healthRatio > 0.5f)
        {
            returnScore = 20;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _agent = agent.GetComponent<NavMeshAgent>();
        _aiHealth = agent.GetComponent<AIHealth>();
    }

    public bool IsFinished()
    {
        //Checks if the enemy is low on health or if the state duration is up
        return (_aiHealth.GetHealthRatio() < 0.5f || _timer >= _duration);

    }
}
