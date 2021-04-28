using System.Collections;
using System.Collections.Generic;
using tgpAudio;
using UnityEngine;

public class AmbientTrigger : MonoBehaviour
{
    public tgpAudio.AudioType _type;

    private AudioController _audioSystem;

    private void Awake()
    {
        _audioSystem = GameObject.FindObjectOfType<AudioController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _audioSystem.PlayAudio(_type, true);
        }
    }
}
