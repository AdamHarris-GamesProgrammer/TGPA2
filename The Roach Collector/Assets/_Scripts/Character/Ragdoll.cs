using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] _rigidbodies;
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        Health health = GetComponent<Health>();

        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in _rigidbodies)
        {
            Hitbox hitbox = rb.gameObject.AddComponent<Hitbox>();
            hitbox._health = health;
            if (hitbox.gameObject != gameObject)
            {
                hitbox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
            }
        }

        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        _animator.enabled = true;
        foreach(var rb in _rigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    public void ActivateRagdoll()
    {
        _animator.enabled = false;
        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = false;
        }
    }
}
