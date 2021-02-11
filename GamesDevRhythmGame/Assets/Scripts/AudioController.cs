using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]

public class AudioController : MonoBehaviour
{

    AudioSource audioSource;
    public static float[] samples = new float[1024];

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        GetAudioSource();
    }


    void GetAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Hanning);
    }
}
