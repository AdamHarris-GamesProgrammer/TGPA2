using Harris.Inventories;
using Harris.UI;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    bool _inRange = false;
    bool _isOpen = false;

    GameObject _chestInventoryPanel;
    GameObject _playerInventoryPanel;
    Inventory _chestInventory;


    void Awake()
    {
        _playerInventoryPanel = FindObjectOfType<ShowHideUI>().UIContainer;
        _chestInventory = GetComponent<Inventory>();

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
            _chestInventoryPanel = FindObjectOfType<PlayerController>().ChestInventory;
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
        //TODO: Open/close player inventory and chest inventory
        _chestInventoryPanel.SetActive(val);
        _playerInventoryPanel.SetActive(val);


        if (!val)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
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
