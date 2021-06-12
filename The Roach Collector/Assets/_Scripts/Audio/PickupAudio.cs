using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAudio : MonoBehaviour
{
    [SerializeField] AudioSource _pickupAudioSource;
    [SerializeField] AudioClip _pickupClip;

    public void PlayPickupSound()
    {
        _pickupAudioSource.PlayOneShot(_pickupClip);
    }
}


