using System.Collections;
using System.Collections.Generic;
using TGP.Movement;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _aimLayerMask;

    private Animator _animator;

    [SerializeField] private float _speed = 5.0f;

    [SerializeField] private float _crouchSpeedFactor = 0.5f;
    [SerializeField] private float _sprintSpeedFactor = 1.5f;


    private PlayerAim _playerAim;

    float _movmentSpeed;
    bool _isCrouched = false;
    bool _isSprinting = false;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movmentSpeed = _speed;

        _playerAim = GetComponent<PlayerAim>();
    }

    private void OnCrouch()
    {
        _isCrouched = !_isCrouched;

        if (_isCrouched)
        {
            Debug.Log("Crouched");
            _movmentSpeed = _speed * _crouchSpeedFactor;
        }
        else
        {
            Debug.Log("Uncrouched");
            _movmentSpeed = _speed;
        }
        _animator.SetBool("isCrouched", _isCrouched);
    }

    private void OnSprint()
    {
        _isSprinting = !_isSprinting;

        if (_isSprinting)
        {
            Debug.Log("Sprinting");
            _isCrouched = false;
            _movmentSpeed = _speed * _sprintSpeedFactor;
        }
        else
        {
            Debug.Log("Not sprinting");
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

    private void FixedUpdate()
    {
        // Reading the Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        Vector3 movement;

        if (_playerAim.GetAiming())
        {
            movement = transform.forward * vertical + transform.right * horizontal;
        }
        else
        {
            movement = Camera.main.transform.forward * vertical + Camera.main.transform.right * horizontal;
        }


        // Moving
        if (movement.magnitude > 0)
        {
            _animator.SetBool("isMoving", true);
            movement = Vector3.ClampMagnitude(movement, 1);
            movement *= _movmentSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);

            if(!_playerAim.GetAiming())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
                {
                    var destination = hitInfo.point;
                    destination.y = transform.position.y;

                    var _direction = destination - transform.position;
                    _direction.y = 0f;
                    _direction.Normalize();
                    transform.rotation = Quaternion.LookRotation(_direction, transform.up);
                }
            }
        }
        else
        {
            _animator.SetBool("isMoving", false);
            _animator.SetBool("isSprinting", false);
        }


        // Animating
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);


        //Debug.Log("Velocity {X: " + velocityX + ", Z: " + velocityZ + "}");


        _animator.SetFloat("velocityZ", velocityZ, 0.1f, Time.deltaTime);
        _animator.SetFloat("velocityX", velocityX, 0.1f, Time.deltaTime);

    }
}
