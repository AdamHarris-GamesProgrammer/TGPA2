using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public AudioSource _buttonClick;

    //resets scene
    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
    
    //goes to hideout scene
    public void Hideout()
    {
        SceneManager.LoadScene("HideOut");
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
