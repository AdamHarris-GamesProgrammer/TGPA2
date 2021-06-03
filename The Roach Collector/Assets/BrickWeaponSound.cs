using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrickWeaponSound : MonoBehaviour
{

    [SerializeField] AudioSource ASrc;
    [SerializeField] bool PlaySound = false;
    [SerializeField] LayerMask EnvLayer = 8;

    public UnityEvent _OnHit;


    WeaponStabCheck wsc;

    private void Start()
    {
        wsc = GetComponentInParent<WeaponStabCheck>();
        ASrc = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == EnvLayer)
        {

        }

        if(wsc.GetStabbing())
        {
            Debug.Log("Hitting environment.");
            if (!PlaySound)
            {
                ASrc.Play();

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
