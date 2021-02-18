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

        AttackState attack;
        PatrolState Patrol;

        public float speed = 2.0f;
        public float detectionRadius = 2.0f;
        
        private void Awake()
        {
            GameObject playerGo = GameObject.Find("Player");
            playerTransform = playerGo.transform;
            
            
        }

        public bool WithinRange;

        protected override void Initialize()
        {
            attack = new AttackState(StateID.Attack, this, speed);
            attack.AddTransition(Transition.PlayerOutsideRange, StateID.Patrol);

            Patrol = new PatrolState(StateID.Patrol, this, detectionRadius);
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


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Waypoint")
            {
                Patrol.WaypointReached(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Waypoint")
            {
                Patrol.WaypointReached(false);
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

    }

    

}
