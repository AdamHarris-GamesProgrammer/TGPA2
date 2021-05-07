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

    FieldOfView _fov;

    LastKnownLocation _lastKnownLocation;

    float _timer = 0.0f;

    public void Action()
    {
        _timer += Time.deltaTime;


        if (_navAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            if (_fov.IsEnemyInFOV)
            {
                //Set the player as the target
                _aiWeapon.SetTarget(_agent.GetPlayer());

                //Start firing
                _aiWeapon.SetFiring(true);
            }
            else
            {
                //TODO: Replace 12.5 with a weapon usability range
                _navAgent.SetDestination(_lastKnownLocation.GeneratePointInRange(12.5f));

                _aiWeapon.SetTarget(null);
                _aiWeapon.SetFiring(false);
                //TODO: Find nearby allies and see if they know where the player is.
                //TODO: Use this to move to that new location and shoot
            }
        }
        else
        {
            _aiWeapon.SetTarget(null);
            _aiWeapon.SetFiring(false);
        }

        //TODO: Decide when is the optimal position to shoot. 
        //TODO: Make the AI decide where to move based on if they can shoot the player from there.
    }


    public void EnterSnippet()
    {
        Debug.Log(_agent.transform.name + " Advance Snippet");

        _timer = 0.0f;

        Vector3 playerPos = _lastKnownLocation.transform.position;

        _navAgent.SetDestination(playerPos);

        //TODO: Change this so some AI will rush the player
        _navAgent.stoppingDistance = 5.0f;
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
        _fov = agent.GetComponent<FieldOfView>();
        _lastKnownLocation = GameObject.FindObjectOfType<LastKnownLocation>();
        _agent = agent;
    }

    public bool IsFinished()
    {
        //Checks if the enemy is low on health or if the state duration is up
        return (_aiHealth.HealthRatio < 0.5f || _timer >= _agent._config._advanceStateDuration);

    }
}
