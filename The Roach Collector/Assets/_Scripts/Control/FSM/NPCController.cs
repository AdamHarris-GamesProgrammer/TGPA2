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

<<<<<<< HEAD
        private FieldOfView _FOV;

=======
>>>>>>> parent of 11fb5c8d (Merge branch 'main' of https://github.com/AdamHarris-GamesProgrammer/TGPA2 into main)
        AttackState attack;
        PatrolState Patrol;

        public float speed = 2.0f;
        public float detectionRadius = 2.0f;
        
        private void Awake()
        {
            GameObject playerGo = GameObject.Find("Player");
            playerTransform = playerGo.transform;
<<<<<<< HEAD
            _FOV = GetComponent<FieldOfView>();
=======
            
            
>>>>>>> parent of 11fb5c8d (Merge branch 'main' of https://github.com/AdamHarris-GamesProgrammer/TGPA2 into main)
        }

        public bool WithinRange;

        protected override void Initialize()
        {
<<<<<<< HEAD

            attack = new AttackState(StateID.Attack, this, speed, detectionRadius);
            attack.AddTransition(Transition.PlayerOutsideRange, StateID.Patrol);


            Patrol = new PatrolState(StateID.Patrol, this, detectionRadius, _FOV);
=======
            attack = new AttackState(StateID.Attack, this, speed);
            attack.AddTransition(Transition.PlayerOutsideRange, StateID.Patrol);

            Patrol = new PatrolState(StateID.Patrol, this, detectionRadius);
>>>>>>> parent of 11fb5c8d (Merge branch 'main' of https://github.com/AdamHarris-GamesProgrammer/TGPA2 into main)
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
