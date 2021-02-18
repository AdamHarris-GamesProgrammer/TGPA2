using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTeleport : MonoBehaviour
{
    private GameObject[] doorway;
    private Vector3 positionFrom;

    private void Awake()
    {
        doorway = GameObject.FindGameObjectsWithTag("Door");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
