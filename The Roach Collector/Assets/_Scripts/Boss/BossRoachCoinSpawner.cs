using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoachCoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject _bossRoachCoin;
    
    //moves boss coin to bosses position on death
    public void SpawnCoin()
    {
        _bossRoachCoin.transform.position = gameObject.transform.position;
    }
}
