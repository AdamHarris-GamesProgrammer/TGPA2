using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TGP.Control
{
    public class NPCController : StateMachine
    {
        public StateID currentState;
        public Transform[] PatrolPoints;
        public NavMeshAgent navAgent;
        
        private void Awake()
        {
            GameObject playerGo = GameObject.Find("Player");
            playerTransform = playerGo.transform;
            
            
        }

        public bool WithinRange;

        protected override void Initialize()
        {
            AttackState attack = new AttackState(StateID.Attack, this);
            attack.AddTransition(Transition.PlayerOutsideRange, StateID.Patrol);

            PatrolState Patrol = new PatrolState(StateID.Patrol, this);
            Patrol.AddTransition(Transition.PlayerDetected, StateID.Engage);
            Patrol.AddTransition(Transition.PlayerLost, StateID.Suspicious);
            Patrol.AddTransition(Transition.PlayerWithinRange, StateID.Attack);

            AddState(Patrol);
            AddState(attack);

        }

        protected override void StateFixedUpdate()
        {
            currentState = CurrentStateID;
            CurrentState.Reason(playerTransform, transform);
            CurrentState.Act(playerTransform, transform);

        }

    }
}
