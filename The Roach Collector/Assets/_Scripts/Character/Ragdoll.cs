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

        var bodies = new HashSet<Rigidbody>(GetComponentsInChildren<Rigidbody>());

        _rigidbodies = bodies.ToArray();

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
            if (rb.gameObject.CompareTag("Enemy")) continue;
            rb.isKinematic = false;
        }
    }
}
