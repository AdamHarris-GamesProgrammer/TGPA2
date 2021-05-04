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
