using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSFX : MonoBehaviour
{

    AudioSource _audioSource;
    [SerializeField] AudioClip _footstepSFX;

    void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    //Plays the footstep sound, called from animator event
    public void Footstep() {
        _audioSource.PlayOneShot(_footstepSFX);
    }
}
