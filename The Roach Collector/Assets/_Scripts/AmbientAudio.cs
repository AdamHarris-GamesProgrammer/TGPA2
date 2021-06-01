using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _ambientSFX;

    void Awake()
    {
        //_audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.Play();
        Debug.Log("Ambient Audio");
    }

}
