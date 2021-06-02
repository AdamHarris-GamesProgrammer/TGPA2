using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public AudioSource _buttonClick;

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    //goes to hideout scene
    public void Hideout()
    {
        SceneManager.LoadScene("Hideout");
    }
    
    //goes to main menu scene
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //makes button click sound
    public void ClickSound()
    {
        _buttonClick.Play();
    }
}
