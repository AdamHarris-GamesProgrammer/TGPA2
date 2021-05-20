using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    public int score;
    public int kills;
    public float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    //calculates final score at the end of the level
    public void EndLevelScoreCalc()
    {
        //no kill bonus
        if (kills == 0)
        {
            score += 10000;
        }

        //time bonus
        if (time < 1 % 60)
        {
            score += 50000;
        }
        else if (time < 2 % 60)
        {
            score += 25000;
        }
        else if (time < 3 % 60)
        {
            score += 10000;
        }
        else if (time < 4 % 60)
        {
            score += 5000;
        }
        else if (time < 5 % 60)
        {
            score += 1000;
        }

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
}
