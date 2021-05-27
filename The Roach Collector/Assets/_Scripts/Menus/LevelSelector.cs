using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    //struct for level save data
    [System.Serializable]
    struct LevelData
    {
        public int highscore;
        public float bestTime;
        public int highestRank;
        public int roachesCollected;
    }

    public int levelID = 0;
    public int levelCount = 5;
    public string[] levelName;
    public float[] levelTimeRecord;
    public int[] levelScore;
    public Sprite[] levelImage;
    public Image levelMenuImage;

    public GameObject LevelMenu;

    public Text LevelNameTXT;
    public Text LevelScoreTXT;
    public Text LevelTimeTXT;
    //public Text LevelNameTXT;

    private void Start()
    {
        //Loads level data
        //FindObjectOfType<SavingWrapper>().Load();
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
    void SetLevelTime(float levelTime)
    {
        LevelTimeTXT.text = levelTime.ToString();
    }

    //sets record score of level in level menu
    void SetLevelScore(int score)
    {
        LevelScoreTXT.text = score.ToString();
    }

    //sets image of level in level menu
    void SetLevelImage(Sprite levelSprite)
    {
        levelMenuImage.sprite = levelSprite;
    }

    void UpdateUI()
    {
        SetLevelName(levelName[levelID]);
        SetLevelTime(levelTimeRecord[levelID]);
        SetLevelScore(levelScore[levelID]);
        SetLevelImage(levelImage[levelID]);
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
            levelID = 0;
        }
        UpdateUI();
    }

    //change to previous level
    public void PreviousLevel()
    {
        if (levelID > 0)
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
        switch (levelID)
        {
            case 0:
                SceneManager.LoadScene("Adam Tut");
                break;
            case 1:
                SceneManager.LoadScene("Level02");
                break;
            case 2:
                SceneManager.LoadScene("Level0" + levelID.ToString());
                break;
        }
    }

    //load level data
    public void Load(object state)
    {

        LevelData data = (LevelData)state;
        levelScore[1] = data.highscore;
        levelTimeRecord[1] = data.bestTime;
        //highestRank = data.highestRank;
        //roachesCollected = data.roachesCollected;
    }
}
