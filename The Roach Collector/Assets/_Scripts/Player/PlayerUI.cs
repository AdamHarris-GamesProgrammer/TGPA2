using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Text ammoTxt;

    [SerializeField] GameObject _alarmText = null;
    [SerializeField] GameObject _unlockDoorPrompt = null;
    [SerializeField] GameObject _assassinationPrompt = null;


    [SerializeField] GameObject _pauseUI;
    private bool isPaused = false;



    private void Awake()
    {
        UpdateAmmoUI(0, 0, 0);
    }

    //update ammo whenever player shoots, reloads or gains ammo. clip is ammo in clip. clipsize is for ammo in each reload and anmoLeft is total ammo
    public void UpdateAmmoUI(int clip, int clipSize, int ammoLeft)
    {
        ammoTxt.text = clip + " / " + (ammoLeft);
        if (ammoLeft > 0)
        {
            ammoTxt.color = Color.white;
        }
        else
        {
            ammoTxt.color = Color.red;
        }
    }

    public void DisplayAlarm(bool val)
    {
        _alarmText.SetActive(val);
    }

    public void DisplayDoorPrompt(bool val)
    {
        _unlockDoorPrompt.SetActive(val);
    }

    public void DisplayAssassinationPrompt(bool val)
    {
        _assassinationPrompt.SetActive(val);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                _pauseUI.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                _pauseUI.SetActive(false);
            }
        }
    }
}
