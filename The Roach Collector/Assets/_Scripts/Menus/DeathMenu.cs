using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public AudioSource _buttonClick;
    public MusicPlayer _musicPlayer;

    private void OnEnable()
    {
        _musicPlayer.Play(1);
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
        FindObjectOfType<SceneLoader>().LoadLevel("Hideout");
    }
    
    //goes to main menu scene
    public void MainMenu()
    {
        FindObjectOfType<SceneLoader>().LoadLevel("MainMenu");
    }

    //makes button click sound
    public void ClickSound()
    {
        _buttonClick.Play();
    }
}
