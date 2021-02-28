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
            _navMeshAgent.speed = _movementSpeed;

            if (_isCrouched)
            {
                _navMeshAgent.speed = _movementSpeed * _crouchSpeedFactor;
            }

            


            float velocityX = 0.0f;
            float velocityZ = 0.0f;

            Vector3 destination = _navMeshAgent.destination;
            Vector3 position = transform.position;

            Vector3 movement = destination - position;


            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {

                movement = Vector3.zero;
                Debug.Log("finished path");
                _animator.SetBool("isMoving", false);
            }

            if (movement.magnitude > 0)
            {

                movement = Vector3.ClampMagnitude(movement, 1);
                movement *= _movementSpeed * Time.deltaTime;

                velocityZ = Vector3.Dot(movement.normalized, transform.forward);
                velocityX = Vector3.Dot(movement.normalized, transform.right);
            }
            else
            {

            }

            _animator.SetFloat("velocityZ", velocityZ, 0.1f, Time.deltaTime);
            _animator.SetFloat("velocityX", velocityX, 0.1f, Time.deltaTime);

        }

        private void Update()
        {


            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 100.0f))
                {
                    MoveTo(hit.point);
                }
            }
        }


        public void MoveTo(Vector3 target)
        {
            _navMeshAgent.SetDestination(target);
            _animator.SetBool("isMoving", true);

        }

    }
}

