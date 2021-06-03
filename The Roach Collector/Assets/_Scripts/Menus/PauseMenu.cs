using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public AudioSource _buttonClick;
    public MusicPlayer _musicPlayer;

    //unlocks cursor
    private void OnEnable()
    {
        _musicPlayer.ChangeVolume(0.25f);
        _musicPlayer.ChangePitch(0.75f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        _musicPlayer.ChangeVolume(0.5f);
        _musicPlayer.ChangePitch(1f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    public void QuitToHideOut()
    {
        SceneManager.LoadScene("Hideout");
    }

    // Update is called once per frame
    public void QuitToDesktop()
    {
        Application.Quit();
    }

    //exits to main menu
    public void ExitMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }

    //makes button click sound
    public void ClickSound()
    {
        _buttonClick.Play();
    }
}
