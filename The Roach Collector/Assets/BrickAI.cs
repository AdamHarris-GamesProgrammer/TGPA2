using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using TGP.Control;

public class BrickAI : AIAgent
{
    static Transform _player;

    public LayerMask CharacterMask { get { return _characterMask; } }
    public Transform Player { get { return _player; } }

    private void Awake()
    {
        _aiHealth = GetComponent<Health>();
        _audioSource = GetComponent<AudioSource>();
        _owningZone = GetComponentInParent<CombatZone>();
        _defaultMoveSpeed = GetComponent<NavMeshAgent>().speed;
        _lastKnownLocation = FindObjectOfType<LastKnownLocation>();

        //Error Logging
        if (!_aiHealth) Debug.LogError(gameObject.name + " has no derivative of Health Component attached to it");
        if (!_audioSource) Debug.LogError(gameObject.name + " has no AudioSource component attached to it");
        if (!_owningZone) Debug.LogError(gameObject.name + " has no CombatZone component in parent GameObject");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Find the player
        if (!_player) _player = GameObject.FindGameObjectWithTag("Player").transform;

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIDeathState(this));
        stateMachine.RegisterState(new BrickAIIdleState(this));
        stateMachine.RegisterState(new AIMeleeState(this));

        if (_startingWeapon)
        {
            RaycastWeapon weapon = Instantiate(_startingWeapon.Weapon);

            _aiWeapon = GetComponent<AIWeapons>();
            _aiWeapon.EquipWeapon(weapon);
        }

        stateMachine.ChangeState(_initialState);
    }

    // Update is called once per frame
    void Update()
    {
        if (_beingKilled) return;

        //If we are dead
        if (_aiHealth.IsDead)
        {
            //Change to death state, activates ragdoll and drops weapons
            stateMachine.ChangeState(AiStateId.Death);

            _player.GetComponent<PlayerController>().AgentInRange = null;
            //Destroy the assassination target component and this ai agent component
            Destroy(GetComponentInChildren<AssassinationTarget>());
            Destroy(this);
        }

        stateMachine.Update();
        _currentState = stateMachine._currentState;

        if (_player.GetComponent<PlayerHealth>().IsDead)
        {
            //Dances, and then stops the AIn from firing and clears the target
            GetComponent<Animator>().SetBool("PlayerDead", true);
            GetComponent<NavMeshAgent>().SetDestination(transform.position);

            _aiWeapon?.SetFiring(false);
            _aiWeapon?.SetTarget(null);
        }
    }

    public override void Aggrevate()
    {
        if (_aiHealth.IsDead) return;

        //Move the last known location to the players position when the player shoots. 
        _lastKnownLocation.transform.position = _player.position;

        _isAggrevated = true;

        stateMachine.ChangeState(AiStateId.Melee);
    }
}

