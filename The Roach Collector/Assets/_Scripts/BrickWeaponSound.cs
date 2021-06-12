using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrickWeaponSound : MonoBehaviour
{

    [SerializeField] bool PlaySound = false;
    [SerializeField] LayerMask EnvLayer = 8;

    public UnityEvent _OnHit;


    WeaponStabCheck wsc;

    private void Start()
    {
        wsc = GetComponentInParent<WeaponStabCheck>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (wsc.GetStabbing())
        {
            if (!PlaySound)
            {
                FindObjectOfType<RandomSoundGenerator>().PlayRandomAudioClip();

                PlaySound = true;

                _OnHit.Invoke();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        PlaySound = false;
    }
}
