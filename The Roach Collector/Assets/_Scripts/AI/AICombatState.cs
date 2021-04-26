using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICombatState : AIState
{
    NavMeshAgent _agent;

    Health _aiHealth;
    AIWeapons _aiWeapon;


    GameObject[] _enemiesInScene;
    List<AIAgent> _enemiesInRange;

    public AICombatState(AIAgent agent)
    {
        _agent = agent.GetComponent<NavMeshAgent>();
        _aiHealth = agent.GetComponent<Health>();
        _aiWeapon = agent.GetComponent<AIWeapons>();

        _enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        _enemiesInRange = new List<AIAgent>();
    }

    public void Update(AIAgent agent)
    {

        int scoreFromCover = CoverEvaluator(agent);
        int scoreFromAdvance = AdvanceEvaluator(agent);
        int scoreFromReload = ReloadEvaluator(agent);
        int scoreFromSetAlarm = SetAlarmEvaluator(agent);
        int scoreFromRetreat = RetreatEvaluator(agent);
        int scoreFromBackup = BackupEvaluator(agent);

        //Debug.Log("Health Ratio: " + _aiHealth.GetHealthRatio());
        //TODO: Switch this to a much better system using BehaviourSnippets, this will do for now
        if (scoreFromCover > scoreFromAdvance &&
            scoreFromCover > scoreFromReload &&
            scoreFromCover > scoreFromSetAlarm &&
            scoreFromCover > scoreFromRetreat &&
            scoreFromCover > scoreFromBackup)
        {
            //Debug.Log("Cover Action");

            CoverAction(agent);
        }
        else if (scoreFromAdvance > scoreFromCover &&
            scoreFromAdvance > scoreFromReload &&
            scoreFromAdvance > scoreFromSetAlarm &&
            scoreFromAdvance > scoreFromRetreat &&
            scoreFromCover > scoreFromBackup)
        {
            //Debug.Log("Advance Action");
            AdvanceAction(agent);
        }
        else if(scoreFromReload > scoreFromCover &&
            scoreFromReload > scoreFromAdvance &&
            scoreFromReload > scoreFromSetAlarm &&
            scoreFromReload > scoreFromRetreat &&
            scoreFromReload > scoreFromBackup)
        {
            ReloadAction(agent);
        }
        else if(scoreFromSetAlarm > scoreFromCover &&
            scoreFromSetAlarm > scoreFromAdvance &&
            scoreFromSetAlarm > scoreFromReload &&
            scoreFromSetAlarm > scoreFromRetreat &&
            scoreFromSetAlarm > scoreFromBackup)
        {
            SetAlarmAction(agent);
        }
        else if(scoreFromRetreat > scoreFromCover &&
            scoreFromRetreat > scoreFromAdvance &&
            scoreFromRetreat > scoreFromReload &&
            scoreFromRetreat > scoreFromSetAlarm &&
            scoreFromRetreat > scoreFromBackup)
        {
            RetreatAction(agent);
        }
        else if(scoreFromBackup > scoreFromCover &&
            scoreFromBackup > scoreFromAdvance &&
            scoreFromBackup > scoreFromSetAlarm && 
            scoreFromBackup > scoreFromReload && 
            scoreFromBackup > scoreFromRetreat)
        {
            BackupAction(agent);
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

        //Debug.Log("Cover evaluator returning: " + returnScore);

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

        //Debug.Log("Advance evaluator returning: " + returnScore);

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

    private int RetreatEvaluator(AIAgent agent)
    {
        int returnScore = 0;

        bool shouldRetreat = (UnityEngine.Random.Range(0.0f, 1.0f) < agent._config._retreatChance);

        //We will retreat if our health ratio is less than 0.1f and we pass the retreat check
        if(_aiHealth.GetHealthRatio() < 0.1f && shouldRetreat)
        {
            //Until I figure out how to build a retreat the ai will just never succeed the retreat check
            //returnScore = 110;
        }

        return returnScore;
    }

    private void RetreatAction(AIAgent agent)
    {
        //TODO: Research retreat systems

        Vector3 desiredVelocity = Vector3.Normalize(agent.transform.position - agent.GetPlayer().position) * 15.0f;
    }

    private int BackupEvaluator(AIAgent agent)
    {
        int returnScore = 0;

        foreach(GameObject enemy in _enemiesInScene)
        {
            if(Vector3.Distance(agent.transform.position, enemy.transform.position) < 25.0f)
            {
                //Added the enemy
                _enemiesInRange.Add(enemy.GetComponent<AIAgent>());
            }
        }

        if (_enemiesInRange.Count > 3) 
        {
            return 100;
        }

        return returnScore;
    }

    private void BackupAction(AIAgent agent)
    {
        foreach(AIAgent enemy in _enemiesInRange)
        {
            //TOOD: Implement aggro method on the agents.
        }
    }

    private int SetAlarmEvaluator(AIAgent agent)
    {
        int returnScore = 0;

        //TODO Implement an alarm 


        return returnScore;

    }

    private void SetAlarmAction(AIAgent agent)
    {

    }

    private int ReloadEvaluator(AIAgent agent)
    {
        int returnScore = 0;

        //Implement the concept of ammo usage


        return returnScore;
    }

    private void ReloadAction(AIAgent agent)
    {

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
