using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndLevelUI : MonoBehaviour
{
    public string LevelName;
    public Text LevelNameTXT;
    public Text Score;
    public Text Time;
    public int currentLevel;
    public Image RoachToken1;
    public Image RoachToken2;
    public Image RoachToken3;
    public Sprite RoachTokenSprite;
    public int roachCount;

    public void NextLevel()
    {
        SceneManager.LoadScene("Level0" + (currentLevel + 1).ToString());
    }

    public void GoToHideout() 
    {
        SceneManager.LoadScene("HideOut");
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Level" + currentLevel.ToString());
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SetLevelName();
        SetRoaches();
    }

    void GetBestScore()
    {

    }

    void GetBestTime()
    {

    }

    void SetBestScore()
    {

    }

    void SetBestTime()
    {

    }

    void SetLevelName()
    {
        LevelNameTXT.text = LevelName; 
    }

    void GetRoaches()
    {

    }

    void SetRoaches()
    {
        switch (roachCount)
        {
            case 0:
                //do nothing
            break;
            case 1:
                RoachToken1.sprite = RoachTokenSprite;
            break;
            case 2:
                RoachToken1.sprite = RoachTokenSprite;
                RoachToken2.sprite = RoachTokenSprite;
            break;
            case 3:
                RoachToken1.sprite = RoachTokenSprite;
                RoachToken2.sprite = RoachTokenSprite;
                RoachToken3.sprite = RoachTokenSprite;
            break;
            default:
            break;

        }
    }
}
