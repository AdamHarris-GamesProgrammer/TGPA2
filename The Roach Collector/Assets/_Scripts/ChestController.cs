using Harris.UI;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    bool _inRange = false;
    bool _isOpen = false;

    GameObject _chestInventory;
    GameObject _playerInventory;



    void Awake()
    {
        //Get the players inventory
        _playerInventory = FindObjectOfType<ShowHideUI>().UIContainer;
    }

    void Update()
    {
        if(_inRange)
        {
            //Do we want to open the chest
            if(Input.GetKeyDown(KeyCode.E))
            {
                //Set is open to not is open, inverting the variable
                _isOpen = !_isOpen;

                Panels(_isOpen);
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Get the Chest inventory from the player
            _chestInventory = FindObjectOfType<PlayerController>().ChestInventory;
            _inRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //No longer in range disable the panels
            _inRange = false;
            Panels(false);
        }
    }

    void Panels(bool val)
    {
        //Enable/Disable the panels
        _chestInventory.SetActive(val);
        _playerInventory.SetActive(val);

        //Handle cursor and timescale issues
        if (!val)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            //Debug.Log("Chest Cursor lock");
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
