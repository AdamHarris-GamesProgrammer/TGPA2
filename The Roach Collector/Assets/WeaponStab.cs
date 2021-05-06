﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStab : MonoBehaviour
{
    [SerializeField] private Health AttackedHealth;
    [SerializeField] private GameObject Parent;
    [SerializeField] private GameObject Player;

    [SerializeField] private WeaponStabCheck _wsCheck;
    [SerializeField] private bool WSC = false;


    private void Start()
    {
        Player =  GameObject.Find("Player");

        if (transform.root.tag == "Enemy")
        {
            _wsCheck = transform.root.GetComponent<WeaponStabCheck>();
        }
        else
        {
            _wsCheck = Player.GetComponent<WeaponStabCheck>();
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        WSC = _wsCheck.GetStabbing();

        if (collision.transform.root.name == "Core")
        {
            Parent = Player;
        }
        else
        {
            Parent = collision.transform.root.gameObject;
        }


        if (collision.transform.root != transform.root)
        {
            if (Parent == Player)
            {
                AttackedHealth = Parent.GetComponent<PlayerHealth>();

            }
            else
            {
                AttackedHealth = Parent.GetComponent<AIHealth>();
            }

            if (WSC)
            {
                Debug.Log(transform.root.name + " STABBED " + collision.transform.root.name);
                AttackedHealth.TakeDamage(DamageType.MELEE_DAMGE, 100);
            }
        }
        
    }
}
