using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    private int score;
    private int kills;
    private float time;
    private int roaches;

    // On awake
    void Awake()
    {
        //gameObject.SendMessage("AIDead", 250, SendMessageOptions.RequireReceiver);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    //calculates final score at the end of the level
    public void EndLevelScoreCalc()
    {
        //no kill bonus (except boss)
        if (kills == 1)
        {
            score += 10000;
        }
        Debug.Log(time%60);
        //time bonus
        if (time < 60)
        {
            score += 50000;
        }
        else if (time < 120)
        {
            score += 25000;
        }
        else if (time < 180)
        {
            score += 10000;
        }
        else if (time < 240)
        {
            score += 5000;
        }
        else if (time < 300)
        {
            score += 1000;
        }
        score += 5000 * roaches;

    }

    public int LevelScore
    {
        get { return score; } 
    }
    public float LevelTime
    {
        get { return time; }
    }
    public int LevelKills
    {
        get { return kills; }
    }

    public void AIDead(int killScore)
    {
        score += killScore;
        kills++;
    }

    public void AddRoach()
    {
        roaches++;
    }
}
