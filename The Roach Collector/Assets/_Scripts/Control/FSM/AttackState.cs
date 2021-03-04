using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TGP.Combat;
using TGP.Resources;

namespace TGP.Control
{
    public class AttackState : State
    {
        float speed;
        float detectionRadius;

        NPCController Controller;

        Fighter _fighter;

        Health _health;

        public AttackState(StateID id, NPCController controller, float chaseSpeed, float detectionradius) : base(id)
        {
            Controller = controller;
            speed = chaseSpeed;
            detectionRadius = detectionradius;
            _fighter = Controller.transform.GetComponent<Fighter>();
            _health = Controller.transform.GetComponent<Health>();
        }

        public override void OnEntry()
        {
        }

        public override void Reason(Transform player, Transform npc)
        {
            if (Vector3.Distance(player.position, npc.position) > detectionRadius)
            {
                Controller.PerformTransition(Transition.PlayerOutsideRange);
            }



        }

        public override void Act(Transform player, Transform npc)
        {
            //TODO: Add cover, move while shooting logic etc.
            if (_health.IsDead()) return;

            if (player.GetComponent<Health>().IsDead()) return;

            npc.LookAt(player, Vector3.up);

            _fighter.Shoot();
        }




    }
}
