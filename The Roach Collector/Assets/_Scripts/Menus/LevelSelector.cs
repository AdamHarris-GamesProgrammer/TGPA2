using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public int levelID = 1;
    public int levelCount = 5;
    public string[] levelName;
    public string[] levelTimeRecord;
    public int[] levelScore;
    public Image[] levelImage;

    public GameObject LevelMenu;

    public Text LevelNameTXT;
    public Text LevelScoreTXT;
    public Text LevelTimeTXT;
    //public Text LevelNameTXT;

    private void Start()
    {
        UpdateUI();
    }

    //exits menu
    public void ExitMenu()
    {
        LevelMenu.SetActive(false);
    }

    //sets name of level in level menu
    void SetLevelName(string levelName)
    {
        LevelNameTXT.text = levelName;
    }

    //sets record time of level in level menu
    void SetLevelTime(string levelTime)
    {
        LevelTimeTXT.text = levelTime;
    }

    //sets record score of level in level menu
    void SetLevelScore(int score)
    {
        LevelScoreTXT.text = score.ToString();
    }

    //sets image of level in level menu
    void SetLevelImage()
    {

    }

    void UpdateUI()
    {
        SetLevelName(levelName[levelID]);
        SetLevelTime(levelTimeRecord[levelID]);
        SetLevelScore(levelScore[levelID]);
    }

    //change to next level
    public void NextLevel()
    {
        if (levelID < levelCount - 1)
        {
            levelID++;
        }
        else
        {
            levelID = 1;
        }
        UpdateUI();
    }

    //change to previous level
    public void PreviousLevel()
    {
        if (levelID > 1)
        {
            levelID--;
        }
        else
        {
            levelID = levelCount;
        }
        UpdateUI();
    }

    //starts current selected level
    public void StartLevel()
    {
        SceneManager.LoadScene("Level" + levelID);
    }
}
