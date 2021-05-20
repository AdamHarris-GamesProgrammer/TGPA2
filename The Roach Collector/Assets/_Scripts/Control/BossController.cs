using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickAgent : AIAgent
{
    static Animator thisAnim;
    [SerializeField]
    FieldOfView _fov;
    [SerializeField]
    AIHealth _brickHealth;

    Transform _player;

    bool _isAggrevated = true;

    private void Awake()
    {
        thisAnim = GetComponent<Animator>();
        _fov = GetComponent<FieldOfView>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _fov = GetComponent<FieldOfView>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState(this));
        stateMachine.RegisterState(new AIDeathState(this));
        stateMachine.RegisterState(new AIIdleState(this));
        stateMachine.RegisterState(new AIFindWeaponState(this));
        stateMachine.RegisterState(new AICombatState(this));

        stateMachine.ChangeState(_initialState);
        if (_initialState == AiStateId.CombatState)
        {
            Aggrevate();
        }

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        //_CurrentState = stateMachine._currentState;
        if (_fov.IsEnemyInFOV)
        {
            Debug.Log("player detected!");
            thisAnim.SetTrigger("isDetected");
        }
    }

}
