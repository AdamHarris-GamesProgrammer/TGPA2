using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TGP.Control
{
    public class PatrolState : State
    {
        int destNumber = 0;
        float distance = 10;
        NPCController Controller;
        NavMeshAgent navAgent;
        Transform TController;
        Vector3 targetVector;
        Vector3 FinalTarget;
        Transform DestTransform;
        public PatrolState(StateID id, NPCController controller) : base(StateID.Patrol)
        {
            Controller = controller;
            TController = controller.transform;
            navAgent = controller.navAgent;
        }

        public override void Act(Transform player, Transform npc)
        {
            SetDestination();
        }

        public override void Reason(Transform player, Transform npc)
        {
            if(Vector3.Distance(player.position, npc.position) < 5f)
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
                
                navAgent.SetDestination(targetVector);
                distance = navAgent.remainingDistance;

            }

            if (distance <= 0.5f)
            {
                Debug.Log("NEXT " + destNumber);
                distance = 100;
                destNumber++;
            }

            if (FinalTarget == Controller.transform.position)
            {
                destNumber = 0;
            }
            else if(destNumber > pointNum)
            {
                destNumber = 0;
            }


        }

    }
}
