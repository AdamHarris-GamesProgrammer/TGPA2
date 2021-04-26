using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        var bodies = new HashSet<Rigidbody>(GetComponentsInChildren<Rigidbody>());

        _rigidbodies = bodies.ToArray();

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
        Debug.Log("Activating ragdoll");
        _animator.enabled = false;
        foreach (var rb in _rigidbodies)
        {
            //TODO: This is a hacky solution, GetComponentsInChildren also returns the parent objects component, remove this from the hash set however breaks the AI
            if (rb.gameObject.name == "AI") continue;
            rb.isKinematic = false;
        }
    }
}
