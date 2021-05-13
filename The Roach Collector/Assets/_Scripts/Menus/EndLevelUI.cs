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
        CheckRank();
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

    //checks player score against threshholds to find the player's rank
    void CheckRank()
    {
        //F
        if (score >= ranks[0])
        {
            rankImage.sprite = rankSprites[5];
        }
        //D
        else if (score >= ranks[1])
        {
            rankImage.sprite = rankSprites[4];
        }
        //C
        else if (score >= ranks[2])
        {
            rankImage.sprite = rankSprites[3];
        }
        //B
        else if (score >= ranks[3])
        {
            rankImage.sprite = rankSprites[2];
        }
        //A
        else if (score >= ranks[4])
        {
            rankImage.sprite = rankSprites[1];
        }
        //S
        else 
        {
            rankImage.sprite = rankSprites[0];
        }
    }
}
