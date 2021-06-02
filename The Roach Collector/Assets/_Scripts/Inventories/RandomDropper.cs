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

        const int ATTEMPTS = 20;

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

