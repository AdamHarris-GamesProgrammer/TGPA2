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

    [SerializeField] private int _AiClipBullets = 0;
    [SerializeField] private int _AiTotalBullets = 0;

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

    List<AIAgent> _agentsInScene;
    List<AlarmController> _alarmsInScene;
    List<CoverController> _coversInScene;

    CombatZone _owningZone;
    public CombatZone Zone { get { return _owningZone; } }

    bool _beingKilled = false;
    bool _usingMelee = false;

    public bool BeingKilled { get { return _beingKilled; } set { _beingKilled = value; } }

    public bool CanActivateAlarm { get { return _canActivateAlarm; } set { _canActivateAlarm = value; } }
    public bool Aggrevated {  get { return _isAggrevated; } set { _isAggrevated = value; } }

    public Health GetHealth()
    {
        return _aiHealth;
    }

    [SerializeField] private RaycastWeapon _startingWeapon = null;
    [SerializeField] private RaycastWeapon _MeleeWeapon;

    public Transform GetPlayer()
    {
        return _player;
    }

    private void Awake()
    {
        _aiHealth = GetComponent<Health>();
        _audioSource = GetComponent<AudioSource>();

        _agentsInScene = new List<AIAgent>();
        _alarmsInScene = new List<AlarmController>();
        _coversInScene = new List<CoverController>();

        _owningZone = GetComponentInParent<CombatZone>();

        _agentsInScene = GameObject.FindObjectsOfType<AIAgent>().ToList<AIAgent>();
        _alarmsInScene = GameObject.FindObjectsOfType<AlarmController>().ToList<AlarmController>();
        _coversInScene = GameObject.FindObjectsOfType<CoverController>().ToList<CoverController>();
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

    // Update is called once per frame
    void Update()
    {
        if (_beingKilled) return;

        stateMachine.Update();
        _currentState = stateMachine._currentState;

        _AiClipBullets =_aiWeapon.GetEquippedWeapon()._clipAmmo;
        _AiTotalBullets = _aiWeapon.GetEquippedWeapon()._totalAmmo;

        if(_AiClipBullets == 0 && _AiTotalBullets == 0 && !_usingMelee)
        {
            _usingMelee = true;
            Debug.Log("Change Weapon");
            RaycastWeapon meleeweapon = Instantiate(_MeleeWeapon);
            _aiWeapon.EquipWeapon(meleeweapon);

            stateMachine.ChangeState(AiStateId.Melee);

        }

    }

    public void Aggrevate()
    {
        if (_aiHealth.IsDead) return;
        _isAggrevated = true;
        stateMachine.ChangeState(AiStateId.CombatState);
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
