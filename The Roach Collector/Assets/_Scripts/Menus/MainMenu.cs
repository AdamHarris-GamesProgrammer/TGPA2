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
    public GameObject OptionsMenu;

    [SerializeField] private AudioSource _buttonClick;

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

    //makes button click sound
    public void ClickSound()
    {
        _buttonClick.Play();
    }
}
