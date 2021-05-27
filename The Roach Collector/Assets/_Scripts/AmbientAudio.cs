using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip _ambientSFX;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.Play();
        Debug.Log("Ambient Audio");
    }

}
