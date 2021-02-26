using System.Collections;
using System.Collections.Generic;
using TGP.Movement;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnLocation;
    [SerializeField] private LayerMask _aimLayerMask;

    private Mover _mover;
    private Animator _animator;

    [SerializeField] private float _speed = 5.0f;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _animator = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _mover.SetCrouched(!_mover.GetCrouched());
        }

        // Reading the Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = Camera.main.transform.forward * vertical + Camera.main.transform.right * horizontal;
        

        // Moving
        if (movement.magnitude > 0)
        {
            _animator.SetBool("isMoving", true);
            movement = Vector3.ClampMagnitude(movement, 1);
            movement *= _speed * Time.deltaTime;
            transform.Translate(movement, Space.World);

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
        else
        {
            _animator.SetBool("isMoving", false);
        }


        // Animating
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);


        Debug.Log("Velocity {X: " + velocityX + ", Z: " + velocityZ + "}");

        _animator.SetFloat("velocityZ", velocityZ, 0.1f, Time.deltaTime);
        _animator.SetFloat("velocityX", velocityX, 0.1f, Time.deltaTime);

    }
}
