using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10.0f;

    private void Awake()
    {
        Destroy(gameObject, 5.0f);
    }


    private void FixedUpdate()
    {
        Vector3 pos = transform.position;

        pos += transform.forward * _movementSpeed * Time.deltaTime;

        transform.position = pos;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entered");
        Destroy(gameObject);
    }
}
