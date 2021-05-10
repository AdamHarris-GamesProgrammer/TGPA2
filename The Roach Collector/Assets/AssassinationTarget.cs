using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class AssassinationTarget : MonoBehaviour
{
    Transform _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    //Used for telling the player when they are in range for assassination attack
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in range");
            _player.GetComponent<PlayerController>().AgentInRange = GetComponentInParent<AIAgent>();
        }
    }

    //Used for telling the player when they are no longer in range for assassination attack
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player not in range");
            _player.GetComponent<PlayerController>().AgentInRange = null;
        }
    }
}
