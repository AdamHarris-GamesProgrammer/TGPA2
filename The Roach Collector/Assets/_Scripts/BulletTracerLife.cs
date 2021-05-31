using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracerLife : MonoBehaviour
{
    [SerializeField] private float _tracerLife = 2.0f;

    // Update is called once per frame
    void Update()
    {
        _tracerLife -= Time.deltaTime;
        if(_tracerLife <= 0)
        {
            Destroy(gameObject);
        }
    }
}
