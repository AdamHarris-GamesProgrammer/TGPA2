using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AILocomotion : MonoBehaviour
{
    NavMeshAgent _agent;

    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("movementSpeed", _agent.velocity.magnitude);
    }

    public void MoveTo(Vector3 pos)
    {
        _agent.destination = pos;
    }

    public void DisableNavAgent()
    {
        _agent.enabled = false;
    }
}
