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
        Debug.Log("aaaa");
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("_Dev");
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
