using Harris.Saving;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class RoachPickup : MonoBehaviour, ISaveable
{
    bool _isInRange = false;
    bool _isPickedUp = false;


    void Awake()
    {
        
    }

    void Update()
    {
        if (_isPickedUp) return;
        if(_isInRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                FindObjectOfType<PlayerController>().GainRoach(1);
                
                _isPickedUp = true;

                foreach(Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }

                //Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _isInRange = true;
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _isInRange = false;
        }
    }

    public object Save()
    {
        return _isPickedUp;
    }

    public void Load(object state)
    {
        _isPickedUp = (bool)state;

        if(_isPickedUp)
        {
            Destroy(gameObject);
        }
        else
        {

        }
    }
}
