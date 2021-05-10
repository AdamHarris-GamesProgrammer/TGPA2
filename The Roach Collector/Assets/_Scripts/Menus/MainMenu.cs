using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LevelSelectorMenu;
    public GameObject OptionsMenu;

    public void Play()
    {
        LevelSelectorMenu.SetActive(true);
    }
    public void Hideout()
    {
        SceneManager.LoadScene("Hideout");
    }
    public void Setting()
    {
        OptionsMenu.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
