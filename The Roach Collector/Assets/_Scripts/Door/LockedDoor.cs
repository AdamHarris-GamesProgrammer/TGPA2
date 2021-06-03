using System;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.Events;

public class LockedDoor : MonoBehaviour
{
    bool _isUnlocked = false;
    public bool IsUnlocked { get { return _isUnlocked; } }

    [SerializeField] LockedDoorID _id;
    
    PlayerController _player;

    public UnityEvent _OnOpen;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    public LockedDoorID GetLockID()
    {
        return _id;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Set the door in range for the player
        if (other.CompareTag("Player")) _player.DoorInRange = this;
    }

    private void OnTriggerExit(Collider other)
    {
        //Remove the door in range for the player
        if (other.CompareTag("Player")) _player.DoorInRange = null;
    }

    public void Unlock()
    {
        _OnOpen.Invoke();


        //Remove the door in range for the player
        _player.DoorInRange = null;

        //Triggers the unlocking
        _isUnlocked = true;
        //Plays the animation

        //Destroys component
        Destroy(this);
    }
}
   
