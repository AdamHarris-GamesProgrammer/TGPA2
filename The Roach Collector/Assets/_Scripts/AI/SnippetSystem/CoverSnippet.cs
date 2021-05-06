using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoverSnippet : CombatSnippet
{
    AIWeapons _aiWeapon;
    NavMeshAgent _navAgent;
    AIHealth _aiHealth;
    Animator _anim;

    

    CoverController[] _coversInScene;

    CoverController _currentCover;

    AIAgent _agent;

    bool _hasFoundCover = false;

    float _timer = 0.0f;

    float _crouchDuration = 1.5f;

    float _crouchTimer = 0.0f;

    bool _needToReload = false;

    public void Action()
    {
        _timer -= Time.deltaTime;

        //TODO: Implement AI popping their head over cover
        //TODO: Implement AI crouching and un-crouching animation
        //TODO: Make enemy actively decide when to leave cover based on factors(?)

        if(_currentCover)
        {
            if (_needToReload)
            {
                _crouchTimer -= Time.deltaTime;
                _anim.SetBool("isCrouching", true);
                _aiWeapon.SetTarget(null);
                _aiWeapon.SetFiring(false);

                //Debug.Log("RELOADING");

                _aiWeapon.GetEquippedWeapon()._isReloading = true;
            }

            _agent.transform.LookAt(_agent.GetPlayer());
            _hasFoundCover = true;

            Transform _player = _agent.GetPlayer();

            float angle = Vector3.Angle(_agent.transform.forward, _player.forward);

            if(angle < 166.0f)
            {
                //TODO: Stop enemy from shooting until they stand up.
                _anim.SetBool("isCrouching", false);
                _aiWeapon.SetTarget(_player);

                
                _aiWeapon.SetFiring(true);
                _crouchTimer = _crouchDuration;
            }
            else
            {
                _crouchTimer -= Time.deltaTime;
                _anim.SetBool("isCrouching", true);
                _aiWeapon.SetTarget(null);
                _aiWeapon.SetFiring(false);
            }

            //_crouchTimer -= Time.deltaTime;

            if(_crouchTimer <= 0.0f)
            {
                _crouchTimer = _crouchDuration;
                _anim.SetBool("isCrouching", false);
                _aiWeapon.SetTarget(_player);
                _aiWeapon.SetFiring(true);
            }

            //Debug.Log(angle);
        }

        if(!_hasFoundCover)
        {
            _aiWeapon.SetTarget(null);



            //Debug.Log(_coversInScene.Length);
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
                float distance = Vector3.Distance(_agent.transform.position, cover.transform.position);


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
                float distance = Vector3.Distance(_agent.GetPlayer().position, coverPoint.transform.position);

                if (distance > furthestCoverPointDistance)
                {
                    furthestCoverPoint = coverPoint;
                    furthestCoverPointDistance = distance;
                }
            }

            _navAgent.SetDestination(furthestCoverPoint.position);
            //Debug.Log("Set Destination to: " + furthestCoverPoint.position);
            
            _navAgent.stoppingDistance = 1.0f;

            _hasFoundCover = true;
        }
    }

    public void EnterSnippet()
    {
        Debug.Log(_agent.transform.name + " Cover Snippet");
        _hasFoundCover = false;

        _timer = _agent._config._coverDuration;

        //_anim.SetBool("isCrouching", true);

    }

    public int Evaluate()
    {
        int returnScore = 0;

       _needToReload = _aiWeapon.GetEquippedWeapon().NeedToReload();



        float healthRatio = _aiHealth.HealthRatio;

        
        if(healthRatio <= 0.5f && _needToReload)
        {
            returnScore = 120;
        }
        else if (healthRatio <= 0.5f || _needToReload)
        {
            returnScore = 80;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _navAgent = agent.GetComponent<NavMeshAgent>();
        _aiHealth = agent.GetComponent<AIHealth>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _coversInScene = GameObject.FindObjectsOfType<CoverController>();

        //Debug.Log(_coversInScene.Length);

        _anim = agent.GetComponent<Animator>();
        _agent = agent;
    }

    public bool IsFinished()
    {
        //If the ais health is above the threshold or the timer is less than 0 then exit the snippet.
        bool result = (_aiHealth.HealthRatio > _agent._config._coverExitHealthThreashold || _timer <= 0.0f);

        if(result)
        {
            _currentCover?.RemoveUser();
        }

        return result;
    }
}
