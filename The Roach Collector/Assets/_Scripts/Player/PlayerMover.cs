using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        [Header("Crouch Settings")]
        [SerializeField] private float _verticalOffet = -0.4f;
        [Range(0.01f, 1.0f)][SerializeField] private float _crouchSpeedMultiplier = 0.5f;

        //State Variables
        private CharacterController _controller;

        private Animator _animator;

        float _gravity = -9.81f;
        bool _isGrounded;

        Vector3 _velocity;

        bool _isCrouched = false;


        float _multiplier = 1.0f;

        private void Awake()
        {
            //Get the Character Controller
            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            
            //Check if we are grounded
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

            //Set the velocity to -2 if we are grounded
            if(_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }


            //Crouching
            if (Input.GetKeyDown(KeyCode.C))
            {
                Vector3 cameraPosition = Camera.main.transform.position;

                if (_isCrouched)
                {
                    cameraPosition.y -= _verticalOffet;
                    _multiplier = 1.0f;
                }
                else
                {
                    cameraPosition.y += _verticalOffet;
                    _multiplier = _crouchSpeedMultiplier;
                }

                Camera.main.transform.position = cameraPosition;
                _isCrouched = !_isCrouched;
            }



            //Gets the horizontal and vertical movement
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if(input != Vector2.zero)
            {
                _animator.SetBool("Walk", true);
            }
            else
            {
                _animator.SetBool("Walk", false);
            }

            //Calculates the movement vector
            Vector3 move = transform.right * input.x + transform.forward * input.y;

            //Moves us in the X and Z plane
            _controller.Move(move * (_movementSpeed * _multiplier) * Time.deltaTime);

            //Jump Controls
            if(Input.GetButtonDown("Jump") && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            } 

            //Adds gravity to our velocity calculation
            _velocity.y += _gravity * Time.deltaTime;

            //Moves us in the Y axis
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}

