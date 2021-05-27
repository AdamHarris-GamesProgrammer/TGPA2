using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// script for the main menu buttons
/// allows the main menu scene to join up with other scenes
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject LevelSelectorMenu;
    public GameObject OptionsMenu;

    //plays the tutorial level
    public void Play()
    {
        SceneManager.LoadScene("Tutorial");
    }

    //opens the hideout level
    public void Hideout()
    {
        SceneManager.LoadScene("Hideout");
    }

    //opens the settings menu
    public void Setting()
    {
        OptionsMenu.SetActive(true);
    }

    //quits the game to desktop
    public void Quit()
    {
        Application.Quit();
    }
}
