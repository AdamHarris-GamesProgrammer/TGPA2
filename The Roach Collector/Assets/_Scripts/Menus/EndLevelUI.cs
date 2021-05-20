using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Harris.Saving;

[System.Serializable]
struct LevelData
{
    public int highscore;
    public int bestTime;
    public int highestRank;
    public int roachesCollected;        
}

//UI for end level
public class EndLevelUI : MonoBehaviour, ISaveable
{
    public string LevelName;
    public Text LevelNameTXT;
    public Text Score;
    public Text Time;
    public Image RoachToken1;
    public Image RoachToken2;
    public Image RoachToken3;
    public Sprite RoachTokenSprite;
    private int roachCount;
    [SerializeField]private int currentLevel;
    [SerializeField]private int[] ranks = new int [5];
    [SerializeField]private Sprite[] rankSprites = new Sprite [6];
    [SerializeField]private Image rankImage;
    [SerializeField]private int score;
    [SerializeField]private int time;
    [SerializeField]private int rank;


    private int highscore;
    private int bestTime;
    private int highestRank;
    private int roachesCollected; 

    

    public void NextLevel()
    {
        FindObjectOfType<SavingWrapper>().Save();

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
        //FindObjectOfType<SavingWrapper>().Save();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SetLevelName();
        SetRoaches();
        CheckRank();
        Debug.Log(score);
        score+= 1000;
        //FindObjectOfType<SavingWrapper>().Save();
    }

    void SetLevelName()
    {
        LevelNameTXT.text = LevelName; 
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

    //checks player score against threshholds to find the player's rank
    void CheckRank()
    {
        //F
        if (score >= ranks[0])
        {
            rankImage.sprite = rankSprites[5];
            rank = 0;
        }
        //D
        else if (score >= ranks[1])
        {
            rankImage.sprite = rankSprites[4];
            rank = 1;
        }
        //C
        else if (score >= ranks[2])
        {
            rankImage.sprite = rankSprites[3];
            rank = 2;
        }
        //B
        else if (score >= ranks[3])
        {
            rankImage.sprite = rankSprites[2];
            rank = 3;
        }
        //A
        else if (score >= ranks[4])
        {
            rankImage.sprite = rankSprites[1];
            rank = 4;
        }
        //S
        else 
        {
            rankImage.sprite = rankSprites[0];
            rank = 5;
        }
    }

    public object Save()
    {
        
        LevelData data = new LevelData();

        if (score > highscore)
        {
            data.highscore = score;
        }
        if (time > bestTime)
        {
            data.bestTime = time;
        }
        if (roachCount > roachesCollected)
        {
            data.roachesCollected = roachCount;
        }
        if (rank > highestRank)
        {
            data.highestRank = rank;
        }

        return data;
    }

    public void Load(object state)
    {
        
        LevelData data = (LevelData)state;
        highscore = data.highscore;
        bestTime = data.bestTime;
        highestRank = data.highestRank;
        roachesCollected = data.roachesCollected;
    }

    
}
