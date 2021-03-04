using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using TGP.Movement;

namespace TGP.Control
{
    public class PatrolState : State
    {
        float detectionRadius;
        int destNumber = 0;
        float distance = 10;

        NPCController Controller;
        NavMeshAgent navAgent;

        Mover _mover;

        Transform TController;
        Vector3 targetVector;
        Vector3 FinalTarget;
        Transform DestTransform;

        bool targetReached = false;

        public PatrolState(StateID id, NPCController controller, float detectionRange) : base(StateID.Patrol)
        {
            Controller = controller;
            TController = controller.transform;
            navAgent = controller.gameObject.GetComponent<NavMeshAgent>();
            detectionRadius = detectionRange;
<<<<<<< HEAD

            FOV = fov;

            _mover = controller.gameObject.GetComponent<Mover>();
=======
>>>>>>> parent of 11fb5c8d (Merge branch 'main' of https://github.com/AdamHarris-GamesProgrammer/TGPA2 into main)
        }

        public override void Act(Transform player, Transform npc)
        {
            SetDestination();
        }

        public override void Reason(Transform player, Transform npc)
        {
            if(Vector3.Distance(player.position, npc.position) < detectionRadius)
            {
                navAgent.isStopped = true;
                Controller.PerformTransition(Transition.PlayerWithinRange);
            }
        }

        void SetDestination()
        {
            navAgent.isStopped = false;
            int pointNum = Controller.PatrolPoints.Length - 1;

            FinalTarget = new Vector3(Controller.PatrolPoints[pointNum].position.x, TController.position.y, Controller.PatrolPoints[pointNum].position.z);

            if (destNumber < Controller.PatrolPoints.Length)
            {
                //Debug.Log("Dest Num: " + destNumber);
                DestTransform = Controller.PatrolPoints[destNumber];
                targetVector = new Vector3(DestTransform.position.x, TController.position.y, DestTransform.position.z);
<<<<<<< HEAD

                _mover.MoveTo(targetVector);
                distance = navAgent.remainingDistance;
=======
                
                navAgent.SetDestination(targetVector);
                distance = navAgent.remainingDistance;
                

>>>>>>> parent of 11fb5c8d (Merge branch 'main' of https://github.com/AdamHarris-GamesProgrammer/TGPA2 into main)
            }


            if (targetReached)
            {
                Debug.Log("NEXT " + destNumber);
                distance = 100;
                destNumber++;
                targetReached = false;
            }

            //if (FinalTarget == Controller.transform.position)
            //{
            //    destNumber = 0;
            //}
            if (Vector3.Distance(FinalTarget, Controller.transform.position) <= 0.5)
            {
                destNumber = 0;
            }
            else if(destNumber > pointNum)
            {
                destNumber = 0;
            }
            _mover.MoveTo(targetVector);
            distance = navAgent.remainingDistance;


        }

        public void WaypointReached(bool reached)
        {
            targetReached = reached;
        }

    }
}
