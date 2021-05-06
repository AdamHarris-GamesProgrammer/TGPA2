using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
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

    public void Settings()
    {

    }

    public void ExitMenu()
    {

    }
}
