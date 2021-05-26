using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AILocomotion : MonoBehaviour
{
    NavMeshAgent _agent;

    Animator _animator;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("movementSpeed", _agent.velocity.magnitude);

        if(_agent.velocity.magnitude != 0.0f && !_audioSource.isPlaying) {
            GetComponent<FootstepSFX>().Footstep();
        }
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
