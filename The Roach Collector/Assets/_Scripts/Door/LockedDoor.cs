using System;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    bool _isUnlocked = false;
    public bool IsUnlocked { get { return _isUnlocked; } }

    [SerializeField] LockedDoorID _id;
    
    PlayerController _player;

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
        if (other.CompareTag("Player")) _player.DoorInRange = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _player.DoorInRange = null;
    }

    public void Unlock()
    {
        //TODO: Play unlocking sound

        _player.DoorInRange = null;

        Debug.Log("Door Unlocked");

        _isUnlocked = true;
        GetComponent<Animator>().SetTrigger("OpenDoor");
        Destroy(this);
    }
}
   
