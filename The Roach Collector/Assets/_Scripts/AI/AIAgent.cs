using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using TGP.Control;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AiStateId _initialState;
    public AIAgentConfig _config;

    LastKnownLocation _lastKnownLocation;

    AudioSource _audioSource;
    [SerializeField] private AudioClip _backupPrompt = null;
    [SerializeField] private AudioClip _alarmPrompt = null;
    [SerializeField] private AiStateId _currentState = AiStateId.Idle;
    [SerializeField] private LayerMask _characterMask;
    public LayerMask CharacterMask {  get { return _characterMask; } }

    Transform _player;
    AIWeapons _aiWeapon;

    Health _aiHealth;

    bool _canActivateAlarm = false;
    bool _isAggrevated = false;

    CombatZone _owningZone;

    [Header("Patrol Settings")]
    [SerializeField] PatrolRoute _route;
    [SerializeField] float _movementSpeedInPatrol = 1.5f;
    [SerializeField] float _waitAtEachPointDuration = 7.5f;

    float _defaultMoveSpeed = 5.4f;
    public float DefaultMoveSpeed { get { return _defaultMoveSpeed; } }

    public CombatZone Zone { get { return _owningZone; } }

    bool _beingKilled = false;

    public bool BeingKilled { get { return _beingKilled; } set { _beingKilled = value; } }

    public bool CanActivateAlarm { get { return _canActivateAlarm; } set { _canActivateAlarm = value; } }
    public bool Aggrevated {  get { return _isAggrevated; } set { _isAggrevated = value; } }

    public Health GetHealth()
    {
        return _aiHealth;
    }

    [SerializeField] private RaycastWeapon _startingWeapon = null;


    public Transform GetPlayer()
    {
        return _player;
    }

    private void Awake()
    {
        _aiHealth = GetComponent<Health>();
        _audioSource = GetComponent<AudioSource>();
        _owningZone = GetComponentInParent<CombatZone>();
        _defaultMoveSpeed = GetComponent<NavMeshAgent>().speed;
        _lastKnownLocation = FindObjectOfType<LastKnownLocation>();

        if (!_aiHealth) Debug.LogError(gameObject.name + " has no derivative of Health Component attached to it");
        if (!_audioSource) Debug.LogError(gameObject.name + " has no AudioSource component attached to it");
        if (!_owningZone) Debug.LogError(gameObject.name + " has no CombatZone component in parent GameObject");
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState(this));
        stateMachine.RegisterState(new AIDeathState(this));
        stateMachine.RegisterState(new AIIdleState(this));
        stateMachine.RegisterState(new AIFindWeaponState(this));
        stateMachine.RegisterState(new AICombatState(this));
        stateMachine.RegisterState(new AISearchForPlayerState(this));
        stateMachine.RegisterState(new AICheckPlayerState(this));
        stateMachine.RegisterState(new AIMeleeState(this));

        if(_route != null)
        {
            stateMachine.RegisterState(new PatrolState(this, _route, _movementSpeedInPatrol, _waitAtEachPointDuration));
        }

        if (_startingWeapon)
        {
            RaycastWeapon weapon = Instantiate(_startingWeapon);

            _aiWeapon = GetComponent<AIWeapons>();
            _aiWeapon.EquipWeapon(weapon);
        }
        
        stateMachine.ChangeState(_initialState);
        if(_initialState == AiStateId.CombatState)
        {
            Aggrevate();
        }

    }

    public void ReturnToDefaultState()
    {
        stateMachine.ChangeState(_initialState);
    }

    // Update is called once per frame
    void Update()
    {
        if (_beingKilled) return;
        if (_aiHealth.IsDead)
        {
            stateMachine.ChangeState(AiStateId.Death);

            Destroy(GetComponentInChildren<AssassinationTarget>());
            Destroy(this);
        }

        stateMachine.Update();
        _currentState = stateMachine._currentState;

        if(_player.GetComponent<PlayerHealth>().IsDead)
        {
            GetComponent<Animator>().SetBool("PlayerDead", true);
            _aiWeapon.SetFiring(false);
            _aiWeapon.SetTarget(null);
        }
    }

    public void Aggrevate()
    {
        if (_aiHealth.IsDead) return;


        //Move the last known location to the players position when the player shoots. 
        FindObjectOfType<LastKnownLocation>().transform.position = _player.position;

        _isAggrevated = true;
        stateMachine.ChangeState(AiStateId.CombatState);

    }

    public void LookAtPlayer()
    {
        Vector3 direction = _player.transform.position - transform.position;

        Quaternion look = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime);

        transform.rotation = look;
    }

    public void LookAtLastKnownLocation()
    {
        Vector3 direction = _lastKnownLocation.transform.position - transform.position;

        Quaternion look = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime);

       transform.rotation = look;
    }

    public void PlayBackupSound()
    {
        //Debug.Log("Play Backup Prompt");
        _audioSource.PlayOneShot(_backupPrompt);
    }

    public void PlayAlarmPrompt()
    {
        //Debug.Log("Play Alarm Prompt");
        _audioSource.PlayOneShot(_alarmPrompt);
    }


    //Called by Unity Animator in the StealthAttackResponse and BrutalAttackResponse animations
    void DeathAnimEvent()
    {
        Debug.Log("Death event");

        stateMachine.ChangeState(AiStateId.Death);
    }
}

