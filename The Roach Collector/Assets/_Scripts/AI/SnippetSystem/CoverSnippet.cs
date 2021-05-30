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


    CoverController[] _coversInZone;

    CoverController _currentCover;

    AIAgent _agent;


    bool _needToReload = false;


    bool _isStanding = false;

    float _standShootDuration = 0.5f;
    float _standShootTimer = 0.0f;

    float _changeCoverDuration = 5.0f;
    float _changeCoverTimer = 0.0f;

    float _betweenStandingDuration = 3.0f;
    float _betweenStandingTimer = 0.0f;


    private void TakeCover()
    {
        //Crouch down 
        _anim.SetBool("isCrouching", true);

        //Clear the target and stop the ai from shooting
        _aiWeapon.SetTarget(null);
        _aiWeapon.SetFiring(false);

        //Increase cover timer
        _changeCoverTimer += Time.deltaTime;

        //See if we need to Change covers
        if (_changeCoverTimer > _changeCoverDuration) SelectCover();
    }

    public void Action()
    {
        if (_navAgent.remainingDistance < 1.0f)
        {
            //Look at the player when close enough to cover
            _agent.LookAtLastKnownLocation();

            //AI is not standing
            if (!_isStanding)
            {
                _betweenStandingTimer += _betweenStandingDuration;

                //AI needs to reload
                if (_aiWeapon.GetEquippedWeapon().NeedToReload)
                {
                    //Crouch behind cover while reloading
                    _anim.SetBool("isCrouching", true);
                    _aiWeapon.SetTarget(null);
                    _aiWeapon.SetFiring(false);
                    if (!_aiWeapon.GetEquippedWeapon().IsReloading) _aiWeapon.GetEquippedWeapon().Reload();
                }
                //AI Does not need to reload
                else
                {
                    //Check to see if we can hit the player from the cover
                    RaycastHit hit;
                    if (Physics.Raycast(_agent.transform.position + Vector3.up, _agent.Player.position - _agent.transform.position, out hit, 35.0f, _agent.CharacterMask, QueryTriggerInteraction.Ignore)) 
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            if(_betweenStandingTimer > _betweenStandingDuration)
                            {
                                //Clear the standing timer
                                _betweenStandingTimer = 0.0f;

                                //Stand up
                                _isStanding = true;
                                _anim.SetBool("isCrouching", false);
                                
                                //Shoot the player
                                _aiWeapon.SetTarget(_agent.Player);
                                if (!_aiWeapon.GetEquippedWeapon().IsFiring) _aiWeapon.SetFiring(true);
                            }

                            _changeCoverTimer = 0.0f;
                        }
                        //AI cannot feasibly shoot the player
                        else TakeCover();
                        
                    }
                    //AI can not feasibly shoot the player
                    else TakeCover();
                }
            }
            //AI is standing
            else
            {
                _standShootTimer += Time.deltaTime;

                if(_standShootTimer > _standShootDuration)
                {
                    TakeCover();
                }
            }
        }

    }

    private void SelectCover()
    {
        //Stand up
        _anim.SetBool("isCrouching", false);
        _aiWeapon.SetTarget(null);
        _aiWeapon.SetFiring(false);

        //Clear the change cover timer
        _changeCoverTimer = 0.0f;

        //Remove the AI from their current cover
        if (_currentCover) _currentCover.RemoveUser();

        CoverController closestCover = null;
        float closestDistance = 10000.0f;

        //Cycle through each cover in the array
        foreach (CoverController cover in _coversInZone)
        {
            if (_currentCover != null && _currentCover == cover) continue;

            //If the cover is full then don't go to this cover
            if (cover.IsFull) continue;

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
        //Set our new cover
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
                //Get the distance from the player to the cover points
                float distance = Vector3.Distance(_agent.Player.position, coverPoint.transform.position);

                //Find the furthest one
                if (distance > furthestCoverPointDistance)
                {
                    furthestCoverPoint = coverPoint;
                    furthestCoverPointDistance = distance;
                }
            }

            //Move to the cover point
            _navAgent.SetDestination(furthestCoverPoint.position);
        }
    }

    public void EnterSnippet()
    {
        Debug.Log(_agent.transform.name + " Cover Snippet");

        _aiWeapon.SetTarget(null);

        _navAgent.stoppingDistance = 1.0f;

        SelectCover();
    }

    public int Evaluate()
    {
        if(_coversInZone.Length == 0) return 0;    

        if(_aiWeapon.GetEquippedWeapon()) _needToReload = _aiWeapon.GetEquippedWeapon().NeedToReload;

        float healthRatio = _aiHealth.HealthRatio;

        if (healthRatio <= 0.5f && _needToReload) return 120;
        else if (healthRatio <= 0.5f || _needToReload) return 80;


        return 0;
    }

    public void Initialize(AIAgent agent)
    {
        _navAgent = agent.GetComponent<NavMeshAgent>();
        _aiHealth = agent.GetComponent<AIHealth>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _coversInZone = agent.GetComponentInParent<CombatZone>().CoversInZone.ToArray();

        _anim = agent.GetComponent<Animator>();
        _agent = agent;
    }

    public bool IsFinished()
    {
        //Tests to see if we can exit the cover
        bool result = (_aiHealth.HealthRatio > _agent._config._coverExitHealthThreashold);

        //If we can exit the cover then remove this agent from the cover
        if (result) _currentCover?.RemoveUser();

        return result;
    }

}
