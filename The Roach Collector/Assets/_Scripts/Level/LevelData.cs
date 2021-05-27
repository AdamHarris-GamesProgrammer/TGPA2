using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelDataStruct
{
    public int highscore;
    public float bestTime;
    public int highestRank;
    public int roachesCollected;
}

public class LevelData : MonoBehaviour
{
    
    public int CurrentLevel;
    public LevelDataStruct[] levelData;

    //public void Load(object state)
    //{
    //    for (int i = 0; i < levelData.Length; i++)
    //    {
    //        LevelDataStruct[] data = (LevelData)state;
    //        levelScore = data.highscore;
    //        levelTimeRecord[1] = data.bestTime;
    //        //highestRank = data.highestRank;
    //        //roachesCollected = data.roachesCollected;
    //    }
    //}
}
