using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    [Header("Navigation Settings")]
    public float _maxTime = 1.0f;
    public float _minDistance = 1.0f;

    [Header("Perception Settings")]
    public float _maxSightDistance = 5.0f;

    [Header("Combat Settings")]
    public float _attackDistance = 15.0f;
    

    [Header("Set Alarm Settings")]
    public float _alarmUsableEnemyRange = 10.0f;
    public float _alarmUsableRange = 15.0f;

    [Header("Reload Settings")]


    [Header("Cover Settings")]
    public float _coverExitHealthThreashold = 0.5f;
    public float _coverDuration = 2.0f;

    [Header("Backup Settings")]
    public float _backupEnemyDistance = 25.0f;


    [Header("Advance Settings")]
    public float _advanceStateDuration = 1.5f;
    public float _advanceEnterHealthRatio = 0.5f;

    [Header("Retreat Settings")]
    public float _retreatChance = 0.2f;
}
