using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoverSnippet : CombatSnippet
{
    AIWeapons _aiWeapon;
    NavMeshAgent _navAgent;
    AIHealth _aiHealth;
    CoverController[] _coversInScene;

    CoverController _currentCover;

    AIAgent _agent;

    bool _hasFoundCover = false;

    float _timer = 0.0f;

    public void Action()
    {
        _timer -= Time.deltaTime;

        //TODO: Implement AI popping their head over cover
        //TODO: Implement AI crouching and un-crouching animation
        //TODO: Make enemy actively decide when to leave cover based on factors(?)

        if(_currentCover)
        {
            _agent.transform.LookAt(_agent.GetPlayer());
            _hasFoundCover = true;

            //Implement Ducking

            //Shooting logic

            //Get direction from player to enemy

            //use dot product to determine the angle of the player
            //if the player is facing the enemy, duck
            //if the player is not facing the enemy then stand and shoot

            Transform _player = _agent.GetPlayer();

            Vector3 playerDirection = _player.position - _agent.transform.position;
            //if (playerDirection.sqrMagnitude > _agent._config._maxSightDistance * _agent._config._maxSightDistance)
            //{
            //    //Player is too far away
            //    return;
            //}

            Vector3 agentDirection = _agent.transform.forward;

            playerDirection.Normalize();

            float dotProduct = Vector3.Dot(playerDirection, agentDirection);
            //Straight on dont shoot
            //if (dotProduct > 0.0f)
            //{
            //    //TODO Change this to a proper detection system
            //    //_agent.stateMachine.ChangeState(AiStateId.CombatState);

            //}
            //else
            //{
            //    Debug.Log("Not infront");
            //    //Shoot

            //}

            float angle = Vector3.AngleBetween(_agent.transform.forward, _player.forward);

            if(angle < 2.7f)
            {
                Debug.Log("Infront");
                

                _aiWeapon.SetTarget(_player);
                _aiWeapon.SetFiring(true);
            }
            else
            {
                Debug.Log("Looking at AI");

                _aiWeapon.SetTarget(null);
                _aiWeapon.SetFiring(false);
            }

            Debug.Log(angle);
        }

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
                float distance = Vector3.Distance(_agent.transform.position, cover.transform.position);


                //if the distance from this cover is less than the distance from the closest cover
                if (distance < closestDistance)
                {
                    //Set the new closest cover
                    closestCover = cover;
                    closestDistance = distance;

                }
            }

            Debug.Log(_currentCover?.transform.name);

            if(_currentCover != closestCover)
            {
                Debug.Log("Switched to cover: " + closestCover?.transform.name);
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
            
            _navAgent.stoppingDistance = 1.0f;

            _hasFoundCover = true;
        }
    }

    public void EnterSnippet()
    {
        //Debug.Log("Cover Snippet");
        _hasFoundCover = false;

        _timer = _agent._config._coverDuration;

    }

    public int Evaluate()
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
        _navAgent = agent.GetComponent<NavMeshAgent>();
        _aiHealth = agent.GetComponent<AIHealth>();
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _coversInScene = GameObject.FindObjectsOfType<CoverController>();
        _agent = agent;
    }

    public bool IsFinished()
    {
        //If the ais health is above the threshold or the timer is less than 0 then exit the snippet.
        bool result = (_aiHealth.GetHealthRatio() > _agent._config._coverExitHealthThreashold || _timer <= 0.0f);

        if(result)
        {
            _currentCover?.RemoveUser();
        }

        return result;
    }
}
