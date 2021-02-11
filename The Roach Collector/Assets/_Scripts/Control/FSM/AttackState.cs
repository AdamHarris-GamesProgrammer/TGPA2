using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Control
{
    public class AttackState : State
    {
        float speed = 2f;

        NPCController Controller;
        public AttackState(StateID id, NPCController controller) : base(StateID.Attack)
        {
            Controller = controller;
        }

        public override void Reason(Transform player, Transform npc)
        {

            npc.position = Vector3.MoveTowards(npc.position, player.position, speed * Time.deltaTime);

        }

        public override void Act(Transform player, Transform npc)
        {
            if(Vector3.Distance(player.position, npc.position) > 3)
            {
                Controller.PerformTransition(Transition.PlayerOutsideRange);
            }
        }

    }
}
