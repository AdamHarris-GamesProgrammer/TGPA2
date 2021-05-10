using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Harris.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far the pickups can be scattered from the dropper.")]
        [SerializeField] float _scatterDistance = 1.0f;
        [SerializeField] DropLibrary _dropLibrary = null;
        [SerializeField] int _numberOfDrops = 2;

        const int ATTEMPTS = 20;

        public void RandomDrop()
        {
            var drops = _dropLibrary.GetRandomDrops();

            foreach (var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }

        protected override Vector3 GetDropLocation()
        {

            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + (Random.insideUnitSphere * _scatterDistance);

                //Samples the position on the nav mesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;

        }
    }
}

