using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// level end point require canvas for inventory and the end level UI from core, when the end level condition is completed set isBossAlive to true or false
/// </summary>
public class LevelEndPoint : MonoBehaviour
{
    [SerializeField]private GameObject EndLevelUI;
    [SerializeField]private GameObject InventoryUI;
    [SerializeField]private GameObject Spotlight;
    //[SerializeField]private bool isBossAlive;

    //on enter endpoint
    void OnTriggerEnter(Collider levelEndPoint)
    {
        
        InventoryUI.GetComponent<Canvas>().enabled = false;
        EndLevelUI.SetActive(true);
        gameObject.SetActive(false);
        
    }

    //sets end level spotlight on
    public void SetSpotlight()
    {
        Spotlight.SetActive(true);
    }
}
