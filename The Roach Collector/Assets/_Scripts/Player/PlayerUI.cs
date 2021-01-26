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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Update_Lives_UI(lives);
        Update_Amno_UI(clip, clipSize, amnoLeft);
    }

    public void Update_Lives_UI(int lives)
    {
        for (int i = 0; i < lives; i++)
        {
            hearts[i].SetActive(true);
        }
        for (int i = lives; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }
    }

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
