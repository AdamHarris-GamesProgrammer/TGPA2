using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Harris.Saving;

/// <summary>
/// end level UI system with options scene movement between levels, score and time eval as well as saving score and time 
/// </summary>


//struct for level save data
[System.Serializable]
//struct LevelData
//{
//    public int highscore;
//    public float bestTime;
//    public int highestRank;
//    public int roachesCollected;        
//}

//UI for end level
public class EndLevelUI : MonoBehaviour, ISaveable
{
    public LevelStats levelStats;

    public string LevelName;
    public Text LevelNameTXT;
    public Text ScoreTxt;
    public Text TimeTxt;
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
    [SerializeField]private float time;
    [SerializeField]private int rank;

    private int highscore;
    private float bestTime;
    private int highestRank;
    private int roachesCollected;

    [SerializeField] int _levelIndex = 0;

    LevelDataStruct[] _levelStats;
    
    //loads next level
    public void NextLevel()
    {
        SceneManager.LoadScene("Level0" + (currentLevel + 1).ToString());
    }

    //goes to hideout
    public void GoToHideout() 
    {
        SceneManager.LoadScene("HideOut");
    }

    //goes to main menu
    public void MainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    //reloads load from start
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    void Awake()
    {
        //TODO: Change this if we add more levels
        _levelStats = new LevelDataStruct[2];
    }

    void Start()
    {
        //Loads level data
        FindObjectOfType<SavingWrapper>().Load();

        Time.timeScale = 0;

        //unlock cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //sets UI
        SetLevelName();
        SetLevelStats();
        SetRoaches();
        CheckRank();

        //saves level data
        FindObjectOfType<SavingWrapper>().Save();


    }

    //sets level name
    void SetLevelName()
    {
        LevelNameTXT.text = LevelName; 
    }

    //updates number of icons based on roaches collected
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

    private void SetLevelStats()
    {
        //calculate final score
        levelStats.EndLevelScoreCalc();

        //set score UI
        //score = levelStats.score;
        ScoreTxt.text = score.ToString();

        //set time UI
        //time = levelStats.time;
        //int minutes = (int)time / 60; 
        //int seconds = (int)time % 60;
        //int fraction = (int)(time * 100) % 100;
        //TimeTxt.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);

    }

    //save level data
    public object Save()
    {
        Debug.Log("Save");
        _levelStats[_levelIndex].bestTime = 100.0f;


        //TODO: Check for new highscore etc

        return _levelStats;
        
    }

    //load level data
    public void Load(object state)
    {
        _levelStats = (LevelDataStruct[])state;

        Debug.Log(_levelStats[_levelIndex].bestTime);
    }


}
