using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdvanceSnippet : CombatSnippet
{
    public string Name = "Advance Snippet";

    NavMeshAgent _navAgent;
    AIWeapons _aiWeapon;
    AIHealth _aiHealth;

    AIAgent _agent;

    FieldOfView _fov;

    LastKnownLocation _lastKnownLocation;

    float _timer = 0.0f;


    float _stationaryDuration = 5.0f;
    float _stationaryTimer = 0.0f;

    public void Action()
    {
        _timer += Time.deltaTime;

        Vector3 direction = _agent.GetPlayer().position - _agent.transform.position;

        Quaternion look = Quaternion.Slerp(_agent.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * 5.0f);

        _agent.transform.rotation = look;

         if (_aiWeapon.GetEquippedWeapon().NeedToReload)
                {
                    _aiWeapon.SetTarget(null);
                    _aiWeapon.SetFiring(false);
                    if (!_aiWeapon.GetEquippedWeapon().IsReloading)
                    {
                        _aiWeapon.GetEquippedWeapon().Reload();
                    }

                    //Does the AI have any bullets left?
                    if (_aiWeapon.GetEquippedWeapon().TotalAmmo <= 0)
                    {
                        //TODO Switch to melee state here. 
                    }
                }


        if (_navAgent.remainingDistance <= 1.5f)
        {
            _stationaryTimer += Time.deltaTime;

            if(_stationaryTimer > _stationaryDuration)
            {
                _stationaryTimer = 0.0f;
                _navAgent.SetDestination(_lastKnownLocation.GeneratePointInRange(12.5f));
            }
            else
            {
                _aiWeapon.SetTarget(_agent.GetPlayer());
                //Debug.Log("Should shoot");

                if (_aiWeapon.GetEquippedWeapon().NeedToReload)
                {
                    _aiWeapon.GetEquippedWeapon().Reload();
                }
                else
                {
                    if (!_aiWeapon.GetEquippedWeapon().IsFiring)
                    {
                        _aiWeapon.SetFiring(true);
                    }
                }

                
            }
            
        }
        else
        {
            _aiWeapon.SetTarget(null);
            _aiWeapon.SetFiring(false);
        }

        //TODO: Decide when is the optimal position to shoot. 
        //TODO: Make the AI decide where to move based on if they can shoot the player from there.
        if (_aiWeapon.GetEquippedWeapon()._clipAmmo > 0)
        {
            //Start firing
            _aiWeapon.SetFiring(true);
        }
        else
        {
            _aiWeapon.SetFiring(false);
        }
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
        return (_aiHealth.HealthRatio < 0.5f/* || _timer >= _agent._config._advanceStateDuration*/);

    }

    public string GetName()
    {
        return Name;
    }

}
