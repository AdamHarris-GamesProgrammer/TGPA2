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

    LastKnownLocation _lastKnownLocation;

    AIAgent _agent;


    bool _needToReload = false;


    bool _isStanding = false;

    float _standShootDuration = 0.5f;
    float _standShootTimer = 0.0f;

    float _changeCoverDuration = 5.0f;
    float _changeCoverTimer = 0.0f;




    public void Action()
    {
        //TODO: Implement AI popping their head over cover
        //TODO: Implement AI crouching and un-crouching animation
        //TODO: Make enemy actively decide when to leave cover based on factors(?)


        if (_navAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            Vector3 direction = _lastKnownLocation.transform.position - _agent.transform.position;

            Quaternion look = Quaternion.Slerp(_agent.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime);

            _agent.transform.rotation = look;

            Transform _player = _agent.GetPlayer();

            //float angle = Vector3.Angle(_agent.transform.forward, _player.forward);
            //if (angle < 166.0f)
            //{
            //    //TODO: Stop enemy from shooting until they stand up.
            //    _anim.SetBool("isCrouching", false);
            //    _aiWeapon.SetTarget(_player);
            //    _aiWeapon.SetFiring(true);
            //    _crouchTimer = _crouchDuration;
            //}
            //else
            //{
            //    _crouchTimer -= Time.deltaTime;
            //    _anim.SetBool("isCrouching", true);
            //    _aiWeapon.SetTarget(null);
            //    _aiWeapon.SetFiring(false);
            //}
            ////if (_needToReload)
            ////{
            ////    _crouchTimer -= Time.deltaTime;
            ////    _anim.SetBool("isCrouching", true);
            ////    _aiWeapon.SetTarget(null);
            ////    _aiWeapon.SetFiring(false);
            ////    //Debug.Log("RELOADING");
            ////    _aiWeapon.GetEquippedWeapon()._isReloading = true;
            ////}
            ////_crouchTimer -= Time.deltaTime;
            //if (_crouchTimer <= 0.0f)
            //{
            //    _crouchTimer = _crouchDuration;
            //    _anim.SetBool("isCrouching", false);
            //    _aiWeapon.SetTarget(_player);
            //    _aiWeapon.SetFiring(true);
            //}

            //Todo: implement better standing system.
            //Each action frame perform a raycast from a point above the cover and see if it will hit the player,
            //if the ai does not need to reload then stand up and shoot.
            //or if the ai does need to reload then stay crouched and reload. 


            //if the ai needs to reload 
            //crouch and reload
            //else
            //if raycast from cover to player is successful
            //stand and shoot for 0.5 seconds
            //if the ai has been standing for longer than 0.5 seconds then sit back down and wait to stand for a few seconds
            //else
            //sit still for X amount of time and then go to another piece of cover?
            //therefore the cycle continues until the ai dies

            //AI is not standing
            if (!_isStanding)
            {
                //AI needs to reload
                if (_aiWeapon.GetEquippedWeapon().NeedToReload())
                {
                    _anim.SetBool("isCrouching", true);
                    _aiWeapon.SetTarget(null);
                    _aiWeapon.SetFiring(false);
                    _aiWeapon.GetEquippedWeapon()._isReloading = true;
                }
                //AI Does not need to reload
                else
                {
                    RaycastHit hit;
                    //TODO: replace 25.0f with some kind of weapon range value
                    //AI can feasibly shoot the player
                    if (Physics.Raycast(_agent.transform.position + Vector3.up, _player.position - _agent.transform.position, out hit, 25.0f, _agent.CharacterMask)) 
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            _isStanding = true;
                            _anim.SetBool("isCrouching", false);
                            _aiWeapon.SetTarget(_player);
                            _aiWeapon.SetFiring(true);

                            _changeCoverTimer = 0.0f;
                        }
                        else
                        {
                            Debug.Log("Raycast hit: " + hit.collider.name);

                            _anim.SetBool("isCrouching", true);
                            _aiWeapon.SetTarget(null);
                            _aiWeapon.SetFiring(false);

                            _changeCoverTimer += Time.deltaTime;

                            if (_changeCoverTimer > _changeCoverDuration)
                            {
                                SelectCover();
                            }
                        }
                        
                    }
                    //AI can not feasibly shoot the player
                    else
                    {
                        _anim.SetBool("isCrouching", true);
                        _aiWeapon.SetTarget(null);
                        _aiWeapon.SetFiring(false);

                        _changeCoverTimer += Time.deltaTime;

                        if(_changeCoverTimer > _changeCoverDuration)
                        {
                            SelectCover();
                        }
                    }
                }
            }
            //AI is standing
            else
            {
                _standShootTimer += Time.deltaTime;

                if(_standShootTimer > _standShootDuration)
                {
                    _standShootTimer = 0.0f;
                    _isStanding = false;

                    _anim.SetBool("isCrouching", true);
                    _aiWeapon.SetTarget(null);
                    _aiWeapon.SetFiring(false);
                }
            }
        }

    }

    private void SelectCover()
    {
        _anim.SetBool("isCrouching", false);
        _aiWeapon.SetTarget(null);
        _aiWeapon.SetFiring(false);

        _changeCoverTimer = 0.0f;

        if (_currentCover)
        {
            _currentCover.RemoveUser();
        }

        CoverController closestCover = null;
        float closestDistance = 10000.0f;

        //Cycle through each cover in the array
        foreach (CoverController cover in _coversInScene)
        {
            if (_currentCover != null && _currentCover == cover) continue;

            //If the cover is full then don't go to this cover
            if (cover.IsFull)
            {
                //Debug.Log("Cover is full");
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

        if(_currentCover != null)
        {
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
        }
    }

    public void EnterSnippet()
    {
        Debug.Log(_agent.transform.name + " Cover Snippet");

        _aiWeapon.SetTarget(null);

        _navAgent.stoppingDistance = 1.0f;

        //Debug.Log(_coversInScene.Length);
        SelectCover();


    }

    public int Evaluate()
    {
        int returnScore = 0;

        _needToReload = _aiWeapon.GetEquippedWeapon().NeedToReload();

        float healthRatio = _aiHealth.HealthRatio;

        if (healthRatio <= 0.5f && _needToReload)
        {
            returnScore = 120;
        }
        else if (healthRatio <= 0.5f || _needToReload)
        {
            returnScore = 80;
        }

        Debug.Log("Cover Snippet Returning: " + returnScore);

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _navAgent = agent.GetComponent<NavMeshAgent>();
        _aiHealth = agent.GetComponent<AIHealth>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _coversInScene = GameObject.FindObjectsOfType<CoverController>();
        _lastKnownLocation = GameObject.FindObjectOfType<LastKnownLocation>();

        //Debug.Log(_coversInScene.Length);

        _anim = agent.GetComponent<Animator>();
        _agent = agent;
    }

    public bool IsFinished()
    {
        //TODO: Have a event for when the player has been lost by all AI saying it is safe to come out of cover.
        //If the ais health is above the threshold or the timer is less than 0 then exit the snippet.
        bool result = (_aiHealth.HealthRatio > _agent._config._coverExitHealthThreashold);

        if (result)
        {
            _currentCover?.RemoveUser();
        }

        return result;
    }
}
