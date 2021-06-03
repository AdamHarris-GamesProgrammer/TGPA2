using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource[] _track;
    [SerializeField] private int _currentTrack;

    public void Start()
    {
        _track[_currentTrack].Play();
    }

    //plays chosen track
    public void Play(int trackID)
    {
        _track[trackID].Play();
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
        //_track[_currentTrack].pitch = pitch;
    }

    //change volume of current track
    public void ChangeVolume(float volume)
    {
        _track[_currentTrack].volume = volume;
    }

}
