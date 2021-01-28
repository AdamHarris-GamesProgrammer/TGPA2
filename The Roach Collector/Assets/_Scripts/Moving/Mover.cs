using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TGP.Movement
{
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;


        [SerializeField] private float _movementSpeed = 2.5f;


        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();


            _navMeshAgent.speed = _movementSpeed;
        }

        private void FixedUpdate()
        {
            _animator.SetFloat("movementSpeed", _navMeshAgent.velocity.magnitude);
        }


        public void MoveTo(Vector3 target)
        {
            _navMeshAgent.SetDestination(target);

        }

    }
}

