using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public AudioSource _buttonClick;
    //unlocks cursor
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
        SceneManager.LoadScene("MainMenu");
    }

    //makes button click sound
    public void ClickSound()
    {
        _buttonClick.Play();
    }
}
