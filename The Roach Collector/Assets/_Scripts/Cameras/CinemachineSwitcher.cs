using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{

    public CinemachineVirtualCamera laptopCam;
    public CinemachineFreeLook followCam;


    public BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Laptop Collision");
        SwitchCameraPriority(true);
        

    }
   private void  OnTriggerExit(Collider other)
    {
        SwitchCameraPriority(false);
        Debug.Log("Exiting Laptop Collioder");
       
    }

    private void SwitchCameraPriority(bool openLaptop)
    {
        if (openLaptop)
        {
            followCam.Priority = 0;
            laptopCam.Priority = 1;
            Cursor.visible = true;
            
        }
        else if(!openLaptop)
        {
            Cursor.visible = false;
            followCam.Priority = 1;
            laptopCam.Priority = 0;
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
}
