using Harris.Saving;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class RoachPickup : MonoBehaviour, ISaveable
{
    bool _isInRange = false;
    bool _isPickedUp = false;


    void Update()
    {
        if (_isPickedUp) return;
        if(_isInRange)
        {
            //Are we trying to pick it up
            if(Input.GetKeyDown(KeyCode.E))
            {
                //Gain 1 roach
                FindObjectOfType<PlayerController>().GainRoach(1);
                
                //We have now picked up the roach
                _isPickedUp = true;

                //Disable all child game objects, making the roach invisible
                foreach(Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) _isInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")) _isInRange = false;
    }

    public object Save()
    {
        return _isPickedUp;
    }

    public void Load(object state)
    {
        _isPickedUp = (bool)state;

        if(_isPickedUp) Destroy(gameObject);
    }
}
