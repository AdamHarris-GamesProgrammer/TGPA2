using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public int lives;
    public int clip;
    public int clipSize;
    public int amnoLeft;

    public Text amnoTxt;
    public GameObject[] hearts;

    //update number of lives whenever player is hurt or healed. lives is for the number of lives left
    public void Update_Lives_UI(int lives)
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

    //update amno whenever player shoots, reloads or gains amno. clip is amno in clip. clipsize is for amno in each reload and anmoLeft is total amno
    public void Update_Amno_UI(int clip, int clipsize, int amnoLeft)
    {
        amnoTxt.text = clip + "/ " + (amnoLeft - clipsize);
        if (amnoLeft > 0)
        {
            amnoTxt.color = Color.black;
        }
        else
        {
            amnoTxt.color = Color.red;
        }
        
    }
}
