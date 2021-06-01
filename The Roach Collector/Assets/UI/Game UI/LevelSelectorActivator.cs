using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorActivator : MonoBehaviour
{
    public GameObject _levelSelector;

    //when player enters the activation zone the cursor returns to default and the level selector is made active
    private void OnTriggerEnter(Collider other)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _levelSelector.SetActive(true);
    }
}
