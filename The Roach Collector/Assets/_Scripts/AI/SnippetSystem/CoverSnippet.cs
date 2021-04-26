using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoverSnippet : CombatSnippet
{
    AIWeapons _aiWeapon;
    NavMeshAgent _agent;
    AIHealth _aiHealth;
    CoverController[] _coversInScene;

    CoverController _currentCover;

    bool _hasFoundCover = false;

    public void Action(AIAgent agent)
    {
        if(!_hasFoundCover)
        {
            _aiWeapon.SetTarget(null);


            CoverController closestCover = _coversInScene[0];
            float closestDistance = 10000.0f;

            //Cycle through each cover in the array
            foreach (CoverController cover in _coversInScene)
            {
                //If the cover is full then don't go to this cover
                if (cover.IsFull)
                {
                    Debug.Log("Cover is full");
                    continue;
                }

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

            _currentCover = closestCover;

            //Move the agent to the cover
            _currentCover.AddUser();


            //Find the cover point which is furthest away from the player
            float furthestCoverPointDistance = 0.0f;
            Transform furthestCoverPoint = _currentCover.GetCoverPoints()[0];
            foreach (Transform coverPoint in _currentCover.GetCoverPoints())
            {
                float distance = Vector3.Distance(agent.GetPlayer().position, coverPoint.transform.position);

                if (distance > furthestCoverPointDistance)
                {
                    furthestCoverPoint = coverPoint;
                    furthestCoverPointDistance = distance;
                }
            }

            _agent.SetDestination(furthestCoverPoint.position);
            _agent.stoppingDistance = 1.0f;

            _hasFoundCover = true;
        }
    }

    public void EnterSnippet()
    {
        _hasFoundCover = false;
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
        _coversInScene = GameObject.FindObjectsOfType<CoverController>();
    }

    public bool IsFinished()
    {
        //TODO: Implement finish logic

        bool result = (_aiHealth.GetHealthRatio() > 0.5f);

        if(result)
        {
            _currentCover.RemoveUser();
        }

        return result;
    }
}
