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
        [SerializeField] private float _crouchSpeedFactor = .5f;

        bool _isCrouched = false;

        public bool GetCrouched()
        {
            return _isCrouched;
        }

        public void SetCrouched(bool val)
        {
            _isCrouched = val;
            _animator.SetBool("isCrouched", _isCrouched);
        }

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();

            

            _navMeshAgent.speed = _movementSpeed;
        }

        private void FixedUpdate()
        {
            if (_isCrouched)
            {
                _navMeshAgent.speed = _movementSpeed * _crouchSpeedFactor;
            }
            else
            {
                _navMeshAgent.speed = _movementSpeed;
            }

            _animator.SetFloat("movementSpeed", _navMeshAgent.velocity.magnitude);
        }


        public void MoveTo(Vector3 target)
        {
            _navMeshAgent.SetDestination(target);

        }

    }
}

