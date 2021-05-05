﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void SetTimer(float val)
    {
        _text.text = val.ToString("0.00");
    }
}