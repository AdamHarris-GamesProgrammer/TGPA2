using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] _rigidbodies;
    Animator _animator;

    float _lifeTime = 5.0f;
    float _timer = 0.0f;
    bool _active = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        var bodies = new HashSet<Rigidbody>(GetComponentsInChildren<Rigidbody>());

        _rigidbodies = bodies.ToArray();

        DeactivateRagdoll();
    }

    private void Update()
    {
        if (_active)
        {
            _timer += Time.deltaTime;

            if(_timer > _lifeTime)
            {
                foreach(var rb in _rigidbodies)
                {
                    Destroy(rb.GetComponent<CharacterJoint>());
                    Destroy(rb);
                }

                Destroy(this);
            }
        }
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
        _active = true;
        //Debug.Log("Activating ragdoll");
        _animator.enabled = false;
        foreach (var rb in _rigidbodies)
        {


            if (rb.gameObject.CompareTag("Enemy")) continue;
            rb.isKinematic = false;
        }
    }
}
