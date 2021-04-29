using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public int lives;
    public int clip;
    public int clipSize;
    public int ammoLeft;

    public Text ammoTxt;
    public GameObject[] hearts;

    [SerializeField] GameObject _alarmText;

    //update number of lives whenever player is hurt or healed. lives is for the number of lives left
    public void UpdateLivesUI(int lives)
    {
        //existing lives
        for (int i = 0; i < lives; i++)
        {
            hearts[i].SetActive(true);
        }
        //lost lives
        for (int i = lives; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }
    }

    //update ammo whenever player shoots, reloads or gains ammo. clip is ammo in clip. clipsize is for ammo in each reload and anmoLeft is total ammo
    public void UpdateAmmoUI(int clip, int clipSize, int ammoLeft)
    {
        ammoTxt.text = clip + "/ " + (ammoLeft - clipSize);
        if (ammoLeft > 0)
        {
            ammoTxt.color = Color.black;
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
}
