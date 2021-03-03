using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Movement;
using UnityEngine.AI;

namespace TGP.Movement
{
    public class PlayerMover : MonoBehaviour
    {
        private Animator _animator;

        [SerializeField] private float _speed = 5.0f;

        [Range(0.01f, 1.0f)]
        [Tooltip("The crouch speed multiplier which is used in calculating the final speed of the player")]
        [SerializeField] private float _crouchSpeedFactor = 0.5f;
        [Range(1.0f, 3.0f)]
        [Tooltip("The sprint speed multiplier which is used to calculate the speed of the player while sprinting")]
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


            Vector3 movement = new Vector3();


            if(horizontal != 0.0f || vertical != 0.0f)
            {
                movement = _camera.forward * vertical + _camera.right * horizontal;
            }

            float velocityZ = 0.0f;
            float velocityX = 0.0f;

            // Moving
            if (movement.magnitude > 0)
            {
                _animator.SetBool("isMoving", true);
                movement = Vector3.ClampMagnitude(movement, 1);
                movement *= _movmentSpeed * Time.deltaTime;

                float yPos = transform.position.y;
                transform.Translate(movement, Space.World);

                Vector3 pos = transform.position;
                pos.y = yPos;
                transform.position = pos;

                velocityZ = Vector3.Dot(movement.normalized, transform.forward);
                velocityX = Vector3.Dot(movement.normalized, transform.right);
            }
            else
            {
                _animator.SetBool("isMoving", false);
                _animator.SetBool("isSprinting", false);
                velocityZ = 0.0f;
                velocityX = 0.0f;
            }

            // Animating
            

            _animator.SetFloat("velocityZ", velocityZ, 0.1f, Time.deltaTime);
            _animator.SetFloat("velocityX", velocityX, 0.1f, Time.deltaTime);
        }
    }
}

