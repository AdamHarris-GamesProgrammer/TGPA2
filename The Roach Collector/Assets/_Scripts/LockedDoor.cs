using System;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    bool _isUnlocked = false;

    [SerializeField] LockedDoorID _id;
    
    PlayerController _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }


    private void Update()
    {
        
    }

    public LockedDoorID GetLockID()
    {
        return _id;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.DoorInRange = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _player.DoorInRange = null;
        }
    }

    public void Unlock()
    {
        //TODO: Trigger door opening animation
        //TODO: Activate meshlinks for AI
        //TODO: Play unlocking sound
        //TODO: Disable Player Prompt for unlocking door

        _player.DoorInRange = null;

        Debug.Log("Door Unlocked");

        _isUnlocked = true;

        Destroy(gameObject);
    }
}
   
