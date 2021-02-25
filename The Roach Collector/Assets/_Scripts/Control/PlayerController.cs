using System.Collections;
using System.Collections.Generic;
using TGP.Movement;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnLocation;


    private Mover _mover;
    private Animator _animator;

    [SerializeField] private float _speed = 5.0f;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _mover.SetCrouched(!_mover.GetCrouched());
        }

        // Reading the Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(horizontal, 0f, vertical);

        Vector3 movement = UnityEngine.Camera.main.transform.forward * vertical + UnityEngine.Camera.main.transform.right * horizontal;

        // Moving
        if (movement.magnitude > 0)
        {
            _animator.SetBool("isMoving", true);
            movement.Normalize();
            movement *= _speed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }

        //Vector3 position = UnityEngine.Camera.main.transform.forward * input.y + UnityEngine.Camera.main.transform.right * input.x;

        // Animating
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);

        _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

    }
}
