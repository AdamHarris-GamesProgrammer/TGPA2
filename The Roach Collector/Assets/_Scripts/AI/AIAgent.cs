using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using TGP.Control;

public class AIAgent : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] AiStateId _initialState;
    public AIAgentConfig _config;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip _backupPrompt = null;
    
    [Header("Patrol Settings")]
    [SerializeField] PatrolRoute _route;
    [SerializeField] float _movementSpeedInPatrol = 1.5f;
    [SerializeField] float _waitAtEachPointDuration = 7.5f;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponConfig _startingWeapon = null;

    [Header("Mask Settings")]
    [SerializeField] private LayerMask _characterMask;

    [Header("Debug Settings")]
    [SerializeField] private AiStateId _currentState = AiStateId.Idle;


    public AIStateMachine stateMachine;

    LastKnownLocation _lastKnownLocation;

    AudioSource _audioSource;

    static Transform _player;
    AIWeapons _aiWeapon;

    Health _aiHealth;

    bool _isAggrevated = false;

    CombatZone _owningZone;
    bool _beingKilled = false;


    float _defaultMoveSpeed = 5.4f;

    public LayerMask CharacterMask { get { return _characterMask; } }
    public float DefaultMoveSpeed { get { return _defaultMoveSpeed; } }
    public CombatZone Zone { get { return _owningZone; } }
    public bool BeingKilled { get { return _beingKilled; } set { _beingKilled = value; GetComponent<NavMeshAgent>().isStopped = true; } }
    public bool Aggrevated { get { return _isAggrevated; } set { _isAggrevated = value; } }
    public Health Health { get { return _aiHealth; } }
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
        if(!_player) _player = GameObject.FindGameObjectWithTag("Player").transform;

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIDeathState(this));
        stateMachine.RegisterState(new AIIdleState(this));
        stateMachine.RegisterState(new AICombatState(this));
        stateMachine.RegisterState(new AISearchForPlayerState(this));
        stateMachine.RegisterState(new AICheckPlayerState(this));
        stateMachine.RegisterState(new AIMeleeState(this));

        //Registers the patrol state if we have a patrol route
        if (_route) stateMachine.RegisterState(new PatrolState(this, _route, _movementSpeedInPatrol, _waitAtEachPointDuration));

        //If we have a starting weapon then instantiate and equip it
        if (_startingWeapon)
        {
            RaycastWeapon weapon = Instantiate(_startingWeapon.Weapon);

            _aiWeapon = GetComponent<AIWeapons>();
            _aiWeapon.EquipWeapon(weapon);
        }
        else Debug.LogError(transform.name + " has no starting weapon");

        stateMachine.ChangeState(_initialState);
        if (_initialState == AiStateId.CombatState) Aggrevate();

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

    //Changes the AI back to their default state
    public void ReturnToDefaultState() => stateMachine.ChangeState(_initialState);

    public void Aggrevate()
    {
        if (_aiHealth.IsDead) return;

        //Move the last known location to the players position when the player shoots. 
        _lastKnownLocation.transform.position = _player.position;

        _isAggrevated = true;
        stateMachine.ChangeState(AiStateId.CombatState);

    }

    public void LookAtPlayer()
    {
        //Gets the direction to the player
        Vector3 direction = _player.transform.position - transform.position;
        //Calculates the look vector
        Quaternion look = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * 5.0f);
        //sets our new rotation
        transform.rotation = look;
    }

    public void LookAtLastKnownLocation()
    {
        //Gets the direction to the last known location
        Vector3 direction = _lastKnownLocation.transform.position - transform.position;
        //Calculates the look vector
        Quaternion look = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * 5.0f);
        //Sets our new rotation
        transform.rotation = look;
    }

    //Plays the backup prompt
    public void PlayBackupSound() => _audioSource.PlayOneShot(_backupPrompt);

    //Called by Unity Animator in the StealthAttackResponse and BrutalAttackResponse animations
    void DeathAnimEvent() => stateMachine.ChangeState(AiStateId.Death);
}

