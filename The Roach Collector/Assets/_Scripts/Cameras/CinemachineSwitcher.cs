using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    [Header("Laptop Camera")]
    [SerializeField] CinemachineVirtualCamera laptopCam;
    [Header("Player Cameras")]
    [SerializeField] CinemachineVirtualCamera followCam;
    [SerializeField] CinemachineVirtualCamera aimCam;

    private void OnTriggerEnter(Collider other)
    {
        //Switches to laptop camera
        if(other.CompareTag("Player")) SwitchCameraPriority(true);
    }
   private void  OnTriggerExit(Collider other)
    {
        //Switches to follow camera
        if(other.CompareTag("Player"))  SwitchCameraPriority(false);
    }

    private void SwitchCameraPriority(bool openLaptop)
    {
        if (openLaptop)
        {
            followCam.Priority = 0;
            aimCam.Priority = 0;
            laptopCam.Priority = 1;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
        }
        else
        {
            Cursor.visible = false;
            followCam.Priority = 1;
            aimCam.Priority = 1;
            laptopCam.Priority = 0;
            
        }
    }
}
