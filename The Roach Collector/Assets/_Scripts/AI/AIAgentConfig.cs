using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    public float _maxTime = 1.0f;
    public float _minDistance = 1.0f;
    public float _maxSightDistance = 5.0f;
}
