using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundGenerator : MonoBehaviour
{
    [SerializeField] AudioClip[] _sounds;

    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public AudioClip GetRandomSound()
    {
        //Generates a random index and returns the AudioClip there.
        return _sounds[Random.Range(0, _sounds.Length)];
    }

    public void PlayRandomAudioClip()
    {
        //Plays a sound picked at random
        _audioSource.PlayOneShot(GetRandomSound());
    }
}
