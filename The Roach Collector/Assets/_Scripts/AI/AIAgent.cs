using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AiStateId _initialState;
    public AIAgentConfig _config;

    Transform _player;
    AIWeapons _aiWeapon;



    [SerializeField] private RaycastWeapon _startingWeapon = null;

    public Transform GetPlayer()
    {
        return _player;
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
        stateMachine.RegisterState(new AIAttackPlayerState(this));
        

        if(_startingWeapon == null)
        {
            stateMachine.ChangeState(AiStateId.FindWeapon);
        }
        else
        {
            RaycastWeapon weapon = Instantiate(_startingWeapon);

            _aiWeapon = GetComponent<AIWeapons>();
            _aiWeapon.EquipWeapon(weapon);
            stateMachine.ChangeState(AiStateId.Idle);
        }

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
