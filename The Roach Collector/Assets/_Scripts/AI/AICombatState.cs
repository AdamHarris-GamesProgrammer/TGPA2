using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICombatState : AIState
{
    NavMeshAgent _agent;
    bool _inCover = false;

    Health _aiHealth;
    AIWeapons _aiWeapon;

    public AICombatState(AIAgent agent)
    {
        _agent = agent.GetComponent<NavMeshAgent>();
        _aiHealth = agent.GetComponent<Health>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
    }

    public void Update(AIAgent agent)
    {

        int scoreFromCover = CoverEvaluator(agent);
        int scoreFromAdvance = AdvanceEvaluator(agent);
        int scoreFromReload = 0;
        int scoreFromSetAlarm = 0;
        int scoreFromRetreat = 0;

        Debug.Log("Health Ratio: " + _aiHealth.GetHealthRatio());

        if (scoreFromCover > scoreFromAdvance &&
            scoreFromCover > scoreFromReload &&
            scoreFromCover > scoreFromSetAlarm &&
            scoreFromCover > scoreFromRetreat)
        {
            Debug.Log("Cover Action");

            CoverAction(agent);
        }
        else if (scoreFromAdvance > scoreFromCover &&
            scoreFromAdvance > scoreFromReload &&
            scoreFromAdvance > scoreFromSetAlarm &&
            scoreFromAdvance > scoreFromRetreat)
        {
            Debug.Log("Advance Action");
            AdvanceAction(agent);
        }
        
    }

    private void CoverAction(AIAgent agent)
    {
        _aiWeapon.SetTarget(null);

        GameObject[] coverArr = GameObject.FindGameObjectsWithTag("Cover");

        GameObject closestCover = coverArr[0];
        float closestDistance = 10000.0f;

        //Cycle through each cover in the array
        foreach(GameObject cover in coverArr)
        {
            //Calculate the distance between the agent and the cover
            float distance = Vector3.Distance(agent.transform.position, cover.transform.position);

            //if the distance from this cover is less than the distance from the closest cover
            if (distance < closestDistance)
            {
                //Set the new closest cover
                closestCover = cover;
                closestDistance = distance;
            }
        }

        //Move the agent to the cover
        _agent.SetDestination(closestCover.transform.position);
        _agent.stoppingDistance = 1.0f;
    }

    private int CoverEvaluator(AIAgent agent)
    {
        int returnScore = 0;

        float healthRatio = _aiHealth.GetHealthRatio();

        //TODO: Implement or ammo is less than 35%
        if(healthRatio <= 0.5f)
        {
            returnScore = 100;
        }

        Debug.Log("Cover evaluator returning: " + returnScore);

        return returnScore;
    }

    private int AdvanceEvaluator(AIAgent agent)
    {
        int returnScore = 0;

        float healthRatio = _aiHealth.GetHealthRatio();

        if(healthRatio > 0.5f)
        {
            returnScore = 100;
        }

        Debug.Log("Advance evaluator returning: " + returnScore);

        return returnScore;
    }

    private void AdvanceAction(AIAgent agent)
    {
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

    public AiStateId GetID()
    {
        return AiStateId.CombatState;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("Entering Combat state");
    }

    public void Exit(AIAgent agent)
    {
        Debug.Log("Exiting Combat state");
    }
}
