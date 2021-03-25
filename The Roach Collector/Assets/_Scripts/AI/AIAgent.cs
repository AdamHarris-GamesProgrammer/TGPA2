using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AiStateId _initialState;
    public AIAgentConfig _config;

    [HideInInspector] public NavMeshAgent _agent;
    [HideInInspector] public UIHealthBar _healthBar;
    [HideInInspector] public Ragdoll _ragdoll;
    [HideInInspector] public Transform _player;
    [HideInInspector] public AIWeapons _aiWeapon;
    [HideInInspector] public MeshSockets _sockets;

    [SerializeField] private RaycastWeapon _startingWeapon = null;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _ragdoll = GetComponent<Ragdoll>();
        _healthBar = GetComponentInChildren<UIHealthBar>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _aiWeapon = GetComponent<AIWeapons>();
        _sockets = GetComponent<MeshSockets>();

        stateMachine = new AIStateMachine(this);

        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIFindWeaponState());
        stateMachine.RegisterState(new AIAttackPlayerState());
        

        if(_startingWeapon == null)
        {
            stateMachine.ChangeState(AiStateId.FindWeapon);
        }
        else
        {
            RaycastWeapon weapon = Instantiate(_startingWeapon);

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
