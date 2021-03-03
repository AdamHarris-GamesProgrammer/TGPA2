using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TGP.Control
{
    public class PatrolState : State
    {
        float detectionRadius;
        float detectionAngle;
        LayerMask targetLayer;

        int destNumber = 0;
        float distance = 10;

        NPCController Controller;
        NavMeshAgent navAgent;
        FieldOfView FOV;

        Transform TController;
        Vector3 targetVector;
        Vector3 FinalTarget;
        Transform DestTransform;

        bool targetReached = false;

        public PatrolState(StateID id, NPCController controller, float detectionRange, FieldOfView fov) : base(StateID.Patrol)
        {
            Controller = controller;
            TController = controller.transform;
            navAgent = controller.navAgent;
            detectionRadius = detectionRange;

            FOV = fov;

        }

        public override void Act(Transform player, Transform npc)
        {

            SetDestination();

        }

        public override void Reason(Transform player, Transform npc)
        {
            if (FOV.IsEnemyInFOV())
            {
                navAgent.isStopped = true;
                Controller.PerformTransition(Transition.PlayerWithinRange);
            }


        }

        void SetDestination()
        {
            if (Controller.PatrolPoints.Length == 0) return;

            navAgent.isStopped = false;
            int pointNum = Controller.PatrolPoints.Length - 1;

            FinalTarget = new Vector3(Controller.PatrolPoints[pointNum].position.x, TController.position.y, Controller.PatrolPoints[pointNum].position.z);

            if (destNumber < Controller.PatrolPoints.Length)
            {

                DestTransform = Controller.PatrolPoints[destNumber];
                targetVector = new Vector3(DestTransform.position.x, TController.position.y, DestTransform.position.z);

                navAgent.SetDestination(targetVector);
                distance = navAgent.remainingDistance;


            }


            if (targetReached)
            {
                Debug.Log("NEXT " + destNumber);
                distance = 100;
                destNumber++;
                targetReached = false;
            }

            if (Vector3.Distance(FinalTarget, Controller.transform.position) <= 0.5)
            {
                destNumber = 0;
            }
            else if (destNumber > pointNum)
            {
                destNumber = 0;
            }
            navAgent.SetDestination(targetVector);
            distance = navAgent.remainingDistance;


        }

        public void WaypointReached(bool reached)
        {
            targetReached = reached;
        }

    }
}
