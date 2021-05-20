using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{

    public CinemachineVirtualCamera laptopCam;
    public CinemachineVirtualCamera followCam;
    public CinemachineVirtualCamera aimCam;


    public BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            Debug.Log("Opening Laptop");
            SwitchCameraPriority(true);
        }
        

    }
   private void  OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")) {
            SwitchCameraPriority(false);
            Debug.Log("Exiting Laptop Collioder");
        }
        
       
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
        else if(!openLaptop)
        {
            //Debug.Log("Laptop Cursor lock");
            Cursor.visible = false;
            followCam.Priority = 1;
            aimCam.Priority = 1;
            laptopCam.Priority = 0;
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
}
