using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class AssassinationTarget : MonoBehaviour
{
    Transform _player;

    AIAgent _owner;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _owner = GetComponentInParent<AIAgent>();
    }


    //Used for telling the player when they are in range for assassination attack
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_owner.GetComponent<Health>().IsDead) return;

            //Debug.Log("Player in range");
            _player.GetComponent<PlayerController>().AgentInRange = _owner;
        }
    }

    //Used for telling the player when they are no longer in range for assassination attack
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _player.GetComponent<PlayerController>().AgentInRange = null;
    }
}
