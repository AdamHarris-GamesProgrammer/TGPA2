using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Button _MainMenuButton;
    public Button _HideOutButton;
    public Button _RespawnButton;

    // Start is called before the first frame update
    public void QuitToHideOut()
    {
        SceneManager.LoadScene("HideOut");
    }

    // Update is called once per frame
    public void QuitToDesktop()
    {
        Application.Quit();
    }

    public void Settings()
    {

    }

    public void ExitMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
