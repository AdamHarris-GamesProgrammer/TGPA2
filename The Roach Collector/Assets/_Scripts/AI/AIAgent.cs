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

    AudioSource _audioSource;
    [SerializeField] private AudioClip _backupPrompt = null;
    [SerializeField] private AudioClip _alarmPrompt = null;
    [SerializeField] private AiStateId _currentState = AiStateId.Idle;

    Transform _player;
    AIWeapons _aiWeapon;

    Health _aiHealth;

    bool _canActivateAlarm = false;
    bool _isAggrevated = false;

    List<AIAgent> _agentsInScene;
    List<AlarmController> _alarmsInScene;
    List<CoverController> _coversInScene;

    bool _beingKilled = false;

    public bool BeingKilled { set { _beingKilled = value; } }

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

        _agentsInScene = new List<AIAgent>();
        _alarmsInScene = new List<AlarmController>();
        _coversInScene = new List<CoverController>();

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

    public List<AlarmController> GetAlarmsInRange(float distance)
    {
        List<AlarmController> alarmsInDistance = new List<AlarmController>();

        foreach (AlarmController alarm in _alarmsInScene)
        {
            if (Vector3.Distance(transform.position, alarm.transform.position) < distance)
            {
                alarmsInDistance.Add(alarm);
            }
        }

        return alarmsInDistance;
    }

    public List<AIAgent> GetEnemiesInRange(float distance, bool includeDead = false)
    {
        List<AIAgent> agentsInDistance = new List<AIAgent>();

        if(includeDead)
        {
            foreach (AIAgent enemy in _agentsInScene)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
                {
                    agentsInDistance.Add(enemy);
                }
            }
        }
        else
        {
            foreach (AIAgent enemy in _agentsInScene)
            {
                //Don't add the enemy if the there dead
                if (enemy.GetHealth().IsDead) continue;

                if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
                {
                    agentsInDistance.Add(enemy);
                }
            }
        }

        return agentsInDistance;
    }

    public List<CoverController> GetCoversInRange(float distance)
    {
        List<CoverController> coversInDistance = new List<CoverController>();

        foreach(CoverController cover in _coversInScene)
        {
            if(Vector3.Distance(transform.position, cover.transform.position) < distance)
            {
                coversInDistance.Add(cover);
            }
        }

        return coversInDistance;
    }



    //Called by Unity Animator in the StealthAttackResponse and BrutalAttackResponse animations
    void DeathAnimEvent()
    {
        Debug.Log("Death event");

        stateMachine.ChangeState(AiStateId.Death);
    }
}
