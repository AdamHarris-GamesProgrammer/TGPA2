using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public AudioSource _buttonClick;

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
    
    public void Hideout()
    {
        SceneManager.LoadScene("HideOut");
    }
    
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
