using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Movement;
using UnityEngine.AI;

namespace TGP.Player
{
    public class PlayerMover : MonoBehaviour
    {
        private Animator _animator;

        [SerializeField] private float _speed = 5.0f;

        [SerializeField] private float _crouchSpeedFactor = 0.5f;
        [SerializeField] private float _sprintSpeedFactor = 1.5f;

        Transform _camera;


        private PlayerAim _playerAim;

        float _movmentSpeed;
        bool _isCrouched = false;
        bool _isSprinting = false;

        private void Awake()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            _animator = GetComponent<Animator>();
            _movmentSpeed = _speed;

            _playerAim = GetComponent<PlayerAim>();

            _camera = UnityEngine.Camera.main.transform;
        }


        private void OnCrouch()
        {
            _isCrouched = !_isCrouched;

            if (_isCrouched)
            {
                _movmentSpeed = _speed * _crouchSpeedFactor;
            }
            else
            {
                _movmentSpeed = _speed;
            }
            _animator.SetBool("isCrouched", _isCrouched);
        }

        private void OnSprint()
        {
            _isSprinting = !_isSprinting;

            if (_isSprinting)
            {
                _isCrouched = false;
                _movmentSpeed = _speed * _sprintSpeedFactor;
            }
            else
            {
                _movmentSpeed = _speed;
            }

            _animator.SetBool("isSprinting", _isSprinting);
            _animator.SetBool("isCrouched", _isCrouched);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                OnCrouch();
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                OnSprint();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                OnSprint();
            }
        }

        void FixedUpdate()
        {
            // Reading the Input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");


            Vector3 movement;

            //if (_playerAim.GetAiming())
            //{
            //    movement = transform.forward * vertical + transform.right * horizontal;
            //}
            //else
            //{
            //    movement = _camera.forward * vertical + _camera.right * horizontal;
            //}

            movement = _camera.forward * vertical + _camera.right * horizontal;

            // Moving
            if (movement.magnitude > 0)
            {
                _animator.SetBool("isMoving", true);
                movement = Vector3.ClampMagnitude(movement, 1);
                movement *= _movmentSpeed * Time.deltaTime;
                transform.Translate(movement, Space.World);

            }
            else
            {
                _animator.SetBool("isMoving", false);
                _animator.SetBool("isSprinting", false);
            }

            // Animating
            float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
            float velocityX = Vector3.Dot(movement.normalized, transform.right);

            _animator.SetFloat("velocityZ", velocityZ, 0.1f, Time.deltaTime);
            _animator.SetFloat("velocityX", velocityX, 0.1f, Time.deltaTime);
        }
    }
}

