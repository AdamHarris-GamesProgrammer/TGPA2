using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public int musicVolume;
    public int soundVolume;
    public int mouseSensitivity;

    public Slider musicSlider;
    public Slider soundSlider;
    public Slider mouseSensitivitySlider;

    public GameObject optionsMenu;

    //when music slider is changed
    public void OnMusicChanged()
    {
        musicVolume = (int)musicSlider.value;
    }

    //when sound slider is changed
    public void OnSoundChanged()
    {
        soundVolume = (int)soundSlider.value;
    }

    //when mouse sensitivity slider is changed
    public void OnMouseSensitivityChanged()
    {
        mouseSensitivity = (int)mouseSensitivitySlider.value;
    }

    //close option menu
    public void CloseMenu()
    {
        optionsMenu.SetActive(false);
    }
}
