using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoverSnippet : CombatSnippet
{
    AIWeapons _aiWeapon;
    NavMeshAgent _agent;
    AIHealth _aiHealth;

    public void Action(AIAgent agent)
    {
        _aiWeapon.SetTarget(null);

        GameObject[] coverArr = GameObject.FindGameObjectsWithTag("Cover");

        GameObject closestCover = coverArr[0];
        float closestDistance = 10000.0f;

        //Cycle through each cover in the array
        foreach (GameObject cover in coverArr)
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

    public int Evaluate(AIAgent agent)
    {
        int returnScore = 0;

        float healthRatio = _aiHealth.GetHealthRatio();

        //TODO: Implement or ammo is less than 35%
        if (healthRatio <= 0.5f)
        {
            returnScore = 100;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _agent = agent.GetComponent<NavMeshAgent>();
        _aiHealth = agent.GetComponent<AIHealth>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
    }

    public bool IsFinished()
    {
        //TODO: Implement finish logic


        return (_aiHealth.GetHealthRatio() > 0.5f);
    }
}
