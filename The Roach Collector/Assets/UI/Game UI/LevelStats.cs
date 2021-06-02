using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    private int _score;
    private int _kills;
    private float _time;
    private int _roaches;

    // On awake
    void Awake()
    {
        //gameObject.SendMessage("AIDead", 250, SendMessageOptions.RequireReceiver);
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
    }

    //calculates final score at the end of the level
    public void EndLevelScoreCalc()
    {
        //no kill bonus (except boss)
        if (_kills == 1)
        {
            _score += 10000;
        }

        //time bonus
        if (_time < 60)
        {
            _score += 50000;
        }
        else if (_time < 120)
        {
            _score += 25000;
        }
        else if (_time < 180)
        {
            _score += 10000;
        }
        else if (_time < 240)
        {
            _score += 5000;
        }
        else if (_time < 300)
        {
            _score += 1000;
        }

        //roach token bonus
        _score += 5000 * _roaches;

    }

    //returns level score
    public int LevelScore
    {
        get { return _score; } 
    }

    //returns level time
    public float LevelTime
    {
        get { return _time; }
    }

    //returns level kills
    public int LevelKills
    {
        get { return _kills; }
    }

    //runs each time an enemy in the level is killed
    public void AIDead(int killScore)
    {
        _score += killScore;
        _kills++;
    }

    //add roach when collected
    public void AddRoach()
    {
        _roaches++;
    }
}
