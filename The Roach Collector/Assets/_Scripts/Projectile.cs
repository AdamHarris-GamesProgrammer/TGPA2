using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10.0f;
    float _damage;

    private void Awake()
    {
        Destroy(gameObject, 5.0f);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;

        pos += transform.forward * _movementSpeed * Time.deltaTime;

        transform.position = pos;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Health collisionHealth = collision.gameObject.GetComponent<Health>();

        if(collisionHealth != null)
        {
            collisionHealth.TakeDamage(_damage);
        }


        Destroy(gameObject);
    }
}
