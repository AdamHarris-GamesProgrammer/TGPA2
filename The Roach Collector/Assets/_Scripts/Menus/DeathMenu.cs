using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{ 
    public void Respawn()
    {
        SceneManager.LoadScene("_Dev");
        Debug.Log("Button pressed");
    }
}
