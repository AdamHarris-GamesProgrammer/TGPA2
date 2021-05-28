using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorActivator : MonoBehaviour
{
    public GameObject levelSelector;


    private void OnTriggerEnter(Collider other)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        levelSelector.SetActive(true);
    }
}
