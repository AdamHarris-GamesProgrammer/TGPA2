using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnLocation;


    private NavMeshAgent _agent;

    private void Awake()
    {

    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Instantiate(_bulletPrefab, _bulletSpawnLocation.position, Quaternion.LookRotation(transform.forward));
        //}


        //float angle = Vector3.SignedAngle(target, tf, Vector3.up);

        //Debug.DrawRay(transform.position, transform.forward * 2.0f, Color.green);

        //Debug.Log(angle);

        //if (angle < 45.0f && angle > -45.0f)
        //{
        //    Debug.Log("Forwards");
        //}
        //else if (angle > 45.0f && angle < 135.0f)
        //{
        //    Debug.Log("Right");
        //}
        //else if (angle > 135.0f || angle < -135.0f)
        //{
        //    Debug.Log("Backwards");
        //}
        //else
        //{
        //    Debug.Log("Left");
        //}
    }
}