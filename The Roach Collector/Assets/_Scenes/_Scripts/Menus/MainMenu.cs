using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        //go to level menu
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("_Dev");
    }
    public void Setting()
    {
        //do settings
    }
    public void Quit()
    {
        Application.Quit();
    }
}
