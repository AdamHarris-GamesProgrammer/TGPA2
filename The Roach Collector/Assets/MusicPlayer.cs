using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// for each level track 0 should be the main level the, 1 for game over and 2 for death screen
/// </summary>

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource[] _track;
    [SerializeField] private int _currentTrack;
    [SerializeField] public bool _fadeIn = false;
    [SerializeField] public bool _fadeOut = false;

    [SerializeField] public float _fadeLimit = 0.0f;
    [SerializeField] public float _fadeRate = 0.002f;

    public void Update()
    {
        if (_fadeIn && _track[_currentTrack].volume < _fadeLimit)
        {
            FadeIn(_fadeRate);
        }
        else if (_fadeIn && _track[_currentTrack].volume > _fadeLimit)
        {
            FadeIn(-_fadeRate);
        }
    }

    //plays chosen track
    public void Play(int trackID)
    {
        _track[trackID].Play();
        _currentTrack = trackID;
    }

    //pauses current track
    public void PauseMusic()
    {
        _track[_currentTrack].Pause();
    }

    //unpauses current track
    public void UnpauseMusic()
    {
        _track[_currentTrack].Pause();
    }

    //stops track
    public void StopMusic()
    {
        _track[_currentTrack].Stop();
    }

    //changes pitch of current track
    public void ChangePitch(float pitch)
    {
        _track[_currentTrack].pitch = pitch;
    }

    //change volume of current track
    public void ChangeVolume(float volume)
    {
        _track[_currentTrack].volume = volume;
    }

    public void FadeIn(float delta)
    {
        _track[_currentTrack].volume += delta;
    }

    public void FadeOut(float delta)
    {
        _track[_currentTrack].volume -= delta;
    }

}
