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

    float _stationaryDuration = 5.0f;
    float _stationaryTimer = 0.0f;

    private void DecideToReload()
    {
        //Check if we need to reload
        if (_aiWeapon.GetEquippedWeapon().NeedToReload)
        {
            //Stop firing
            _aiWeapon.SetFiring(false);

            //If we are not reloading, then reload the current gun
            if (!_aiWeapon.GetEquippedWeapon().IsReloading)
            {
                _aiWeapon.GetEquippedWeapon().Reload();
            }

            //If the AI does not have any bullets left, then switch to the melee state
            if (_aiWeapon.GetEquippedWeapon().TotalAmmo <= 0)
            {
                _agent.stateMachine.ChangeState(AiStateId.Melee);
            }
        }
    }

    public void Action()
    {
        //Look at player.
        _agent.LookAtPlayer();

        //Check if we need to reload
        DecideToReload();

        //if we are close to our objective
        if (_navAgent.remainingDistance <= 2.5f)
        {
            //Start Shooting
            if (_fov.IsEnemyInFOV) _aiWeapon.SetFiring(true);
            else _aiWeapon.SetFiring(false);


            //Adds to our stationary timer
            _stationaryTimer += Time.deltaTime;

            //if the AI has been stationary for longer than the duration
            if (_stationaryTimer > _stationaryDuration)
            {
                //Reset the timer and generate a new point in range of the player
                _stationaryTimer = 0.0f;
                _navAgent.SetDestination(_lastKnownLocation.GeneratePointInRangeWithRaycast(12.5f));
            }
            else
            {
                //If we are not firing, then start firing
                if (!_aiWeapon.GetEquippedWeapon().IsFiring) _aiWeapon.SetFiring(true);
            }
        }
    }


    public void EnterSnippet()
    {
        //Debug.Log(_agent.transform.name + " Advance Snippet");

        //Sets the player as our target
        _aiWeapon.SetTarget(_agent.Player);

        //Navigate to a point near the last known location
        _navAgent.SetDestination(_lastKnownLocation.GeneratePointInRangeWithRaycast(12.5f));

        //Set the stopping distance low
        _navAgent.stoppingDistance = 0.5f;
    }

    public int Evaluate()
    {
        if (_aiHealth.HealthRatio > _agent._config._advanceEnterHealthRatio) return 20;

        return 0;
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
        return (_aiHealth.IsDead);
    }

}
