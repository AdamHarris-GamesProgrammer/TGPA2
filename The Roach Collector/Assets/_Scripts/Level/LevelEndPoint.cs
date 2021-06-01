using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// level end point require canvas for inventory and the end level UI from core, when the end level condition is completed set isBossAlive to true or false
/// </summary>
public class LevelEndPoint : MonoBehaviour
{
    [SerializeField]private GameObject _endLevelUI;
    [SerializeField]private GameObject _inventoryUI;
    [SerializeField]private GameObject _spotlight;
    //[SerializeField]private bool isBossAlive;

    //on enter endpoint
    void OnTriggerEnter(Collider levelEndPoint)
    {
        
        _inventoryUI.GetComponent<Canvas>().enabled = false;
        _endLevelUI.SetActive(true);
        gameObject.SetActive(false);
        
    }

    //sets end level spotlight on
    public void SetSpotlight()
    {
        _spotlight.SetActive(true);
    }
}
