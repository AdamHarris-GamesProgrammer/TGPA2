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
        public PatrolState(StateID id, NPCController controller) : base(StateID.Patrol)
        {
            Controller = controller;
            navAgent = controller.navAgent;
        }

        public override void Act(Transform player, Transform npc)
        {
            SetDestination();
        }

        public override void Reason(Transform player, Transform npc)
        {
            if(Vector3.Distance(player.position, npc.position) < 3f)
            {
                navAgent.isStopped = true;
                Controller.PerformTransition(Transition.PlayerWithinRange);
            }
        }

        void SetDestination()
        {
            navAgent.isStopped = false;
            if (destNumber == 2)
            {
                destNumber = 0;
            }

            if (distance == 0)
            {
                distance = 10;
                destNumber++;
            }
            navAgent.SetDestination(Controller.PatrolPoints[destNumber].position);
            distance = navAgent.remainingDistance;

        }

    }
}
