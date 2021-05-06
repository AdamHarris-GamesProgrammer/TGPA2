using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class LastKnownLocation : MonoBehaviour
{
    PlayerController _player;



    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
