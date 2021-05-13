using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndPoint : MonoBehaviour
{
    [SerializeField]private GameObject EndLevelUI;
    [SerializeField]private GameObject Spotlight;
    [SerializeField]private bool isBossAlive;

    void OnTriggerEnter(Collider levelEndPoint)
    {
        if (!isBossAlive)
        {
            EndLevelUI.SetActive(true);
            gameObject.SetActive(false);
        }
        
    }

    public void SetSpotlight()
    {
        Spotlight.SetActive(true);
    }
}
