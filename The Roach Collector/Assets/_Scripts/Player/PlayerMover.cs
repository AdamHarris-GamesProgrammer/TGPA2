using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Movement;
using UnityEngine.AI;

namespace TGP.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [Header("Crouch Settings")]
        [SerializeField] private float _verticalOffet = -0.4f;
        [Range(0.01f, 1.0f)] [SerializeField] private float _crouchSpeedMultiplier = 0.5f;

        Mover _mover;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _mover = GetComponent<Mover>();
        }

        void FixedUpdate()
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


            Vector3 newPos = transform.position;

            Vector3 position = UnityEngine.Camera.main.transform.forward * input.y + UnityEngine.Camera.main.transform.right * input.x;
            newPos += position;

            _mover.MoveTo(newPos);

            //GetComponent<NavMeshAgent>().destination = newPos;

            //GetComponent<Animator>().SetFloat("movementSpeed", GetComponent<NavMeshAgent>().velocity.magnitude);

        }
    }
}

