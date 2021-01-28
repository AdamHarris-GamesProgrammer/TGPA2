using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Movement;

namespace TGP.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Min(0f)] [SerializeField] float _jumpHeight = 2.45f;
        [Min(0f)] [SerializeField] float _movementSpeed = 2.0f;

        [Header("Ground Settings")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundDistance = 0.4f;
        [SerializeField] private LayerMask _groundMask;


        Mover _mover;

        [Header("Crouch Settings")]
        [SerializeField] private float _verticalOffet = -0.4f;
        [Range(0.01f, 1.0f)][SerializeField] private float _crouchSpeedMultiplier = 0.5f;


        private Animator _animator;


        private void Awake()
        {
            //Get the Character Controller
            _animator = GetComponent<Animator>();

            _mover = GetComponent<Mover>();
        }

        void Update()
        {
            //Gets the horizontal and vertical movement
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (input != Vector2.zero)
            {
                _animator.SetBool("Walk", true);
            }
            else
            {
                _animator.SetBool("Walk", false);
            }

            //Calculates the movement vector
            Vector3 move = transform.right * input.x + transform.forward * input.y;


            Vector3 position = transform.position + move;

            _mover.MoveTo(position);
        }
    }
}

