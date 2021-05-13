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
        _playerInventory = FindObjectOfType<ShowHideUI>().UIContainer;

    }

    void Update()
    {
        if(_inRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _isOpen = !_isOpen;

                Panels(_isOpen);
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _chestInventory = FindObjectOfType<PlayerController>().ChestInventory;
            _inRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
            Panels(false);
        }
    }

    void Panels(bool val)
    {
        _chestInventory.SetActive(val);
        _playerInventory.SetActive(val);


        if (!val)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Chest Cursor lock");
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
