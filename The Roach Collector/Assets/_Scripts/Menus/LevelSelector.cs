using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Harris.Saving;

public class LevelSelector : MonoBehaviour
{
    LevelDataStruct levelData;

    //TODO: Change this as level grow
    public int levelCount = 2;
    public int levelID = 0;
    public string[] levelName;
    //public float[] levelTimeRecord;
    //public int[] levelScore;
    //public int[] levelRank;
    //public int[] levelRoaches;
    public Sprite[] levelImage;
    public Image levelMenuImage;

    public GameObject LevelMenu;

    public Text LevelNameTXT;
    //public Text LevelScoreTXT;
    //public Text LevelTimeTXT;

    //[SerializeField] private Sprite[] rankSprites = new Sprite[6];
    //[SerializeField] private Image rankImage;

    //public Image RoachToken1;
    //public Image RoachToken2;
    //public Image RoachToken3;
    //public Sprite RoachTokenSprite;
    //public Sprite RoachEmptyTokenSprite;

    private void Awake()
    {
        //Loads level data
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

    //sets image of level in level menu
    void SetLevelImage(Sprite levelSprite)
    {
        levelMenuImage.sprite = levelSprite;
    }

    

    //updates UI for the level selector
    void UpdateUI()
    {
        SetLevelName(levelName[levelID]);
        SetLevelImage(levelImage[levelID]);
        //SetLevelTime(levelTimeRecord[levelID]);
        //SetLevelScore(levelScore[levelID]);
        //SetRank(levelRank[levelID]);
        //SetRoaches(levelRoaches[levelID]);
    }

    //change to next level
    public void NextLevel()
    {
        levelID = (levelID + 1) % levelCount;

        Debug.Log(levelID);
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

    //load level data from 
    //public void Load(object state)
    //{
    //    Debug.Log("Bee");
    //    LevelDataStruct[] data = new LevelDataStruct[2];
    //    data = (LevelDataStruct[])state;
    //    Debug.Log("Boop: " + data.Length);
    //    for(int i = 0; i < data.Length; i++)
    //    {
    //        levelScore[i] = data[i].highscore;
    //        levelTimeRecord[i] = data[i].bestTime;
    //        Debug.Log(data[i].bestTime);
    //        levelRank[i] = data[i].highestRank;
    //        levelRoaches[i] = data[i].roachesCollected;
    //    }


    //}

    //LevelDataStruct[] data;

    //public object Save()
    //{
    //    data = new LevelDataStruct[2];

    //    for(int i = 0; i < 2; i++)
    //    {
    //        data[i].bestTime = levelTimeRecord[i];
    //        data[i].highscore = levelScore[i];
    //        data[i].highestRank = levelRank[i];
    //        data[i].roachesCollected = levelRoaches[i];
    //    }

    //    return data;
    //}

    //void SetRank(int rank)
    //{
    //    switch (rank)
    //    {
    //        //f
    //        case 0:
    //            rankImage.sprite = rankSprites[5];
    //            break;
    //        //d
    //        case 1:
    //            rankImage.sprite = rankSprites[5];
    //            break;
    //        //c
    //        case 2:
    //            rankImage.sprite = rankSprites[5];
    //            break;
    //        //b
    //        case 3:
    //            rankImage.sprite = rankSprites[5];
    //            break;
    //        //a
    //        case 4:
    //            rankImage.sprite = rankSprites[5];
    //            break;
    //        //s
    //        case 5:
    //            rankImage.sprite = rankSprites[5];
    //            break;
    //    }
    //}

    //void SetRoaches(int roachCount)
    //{
    //    switch (roachCount)
    //    {
    //        case 0:
    //            RoachToken1.sprite = RoachEmptyTokenSprite;
    //            RoachToken2.sprite = RoachEmptyTokenSprite;
    //            RoachToken3.sprite = RoachEmptyTokenSprite;
    //            break;
    //        case 1:
    //            RoachToken1.sprite = RoachTokenSprite;
    //            RoachToken2.sprite = RoachEmptyTokenSprite;
    //            RoachToken3.sprite = RoachEmptyTokenSprite;
    //            break;
    //        case 2:
    //            RoachToken1.sprite = RoachTokenSprite;
    //            RoachToken2.sprite = RoachTokenSprite;
    //            RoachToken3.sprite = RoachEmptyTokenSprite;
    //            break;
    //        case 3:
    //            RoachToken1.sprite = RoachTokenSprite;
    //            RoachToken2.sprite = RoachTokenSprite;
    //            RoachToken3.sprite = RoachTokenSprite;
    //            break;
    //        default:
    //            break;

    //    }
    //}

    ////sets record time of level in level menu
    //void SetLevelTime(float levelTime)
    //{
    //    LevelTimeTXT.text = levelTime.ToString();
    //}

    ////sets record score of level in level menu
    //void SetLevelScore(int score)
    //{
    //    LevelScoreTXT.text = score.ToString();
    //}
}
