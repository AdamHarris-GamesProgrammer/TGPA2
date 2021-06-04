using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    static bool _aggroed = false;
    private MusicPlayer _musicPlayer;

    private void Awake()
    {
        _musicPlayer = FindObjectOfType<MusicPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_aggroed)
            {
                GameObject.FindGameObjectWithTag("Boss").GetComponent<AIAgent>().Aggrevate();
                _aggroed = true;
                _musicPlayer.StopMusic();
                _musicPlayer.Play(2);
                _musicPlayer._fadeLimit = 0.5f;
                _musicPlayer._fadeIn = true;
            }
        }


    }
}
