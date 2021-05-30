using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AILocomotion : MonoBehaviour
{
    NavMeshAgent _agent;
    Animator _animator;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Sets the animator movement speed based on the magnitude of the Agents velocity
        _animator.SetFloat("movementSpeed", _agent.velocity.magnitude);

    }
}
