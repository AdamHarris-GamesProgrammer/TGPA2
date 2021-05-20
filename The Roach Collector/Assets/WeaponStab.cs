using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStab : MonoBehaviour
{
    [SerializeField] private Health AttackedHealth;
    [SerializeField] private GameObject Parent;
    [SerializeField] private GameObject Player;

    [SerializeField] private WeaponStabCheck _wsCheck;
    [SerializeField] private bool WSC = false;
    [SerializeField] WeaponConfig MeleeConfig;


    private void Awake()
    {
        Player =  GameObject.Find("Player");

        if (transform.root.name == "Enemies")
        {
            _wsCheck = GetComponentInParent<AIAgent>().GetComponent<WeaponStabCheck>();
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
        else if(collision.transform.root.name == "Enemies")
        {
            Parent = collision.collider.GetComponentInParent<AIAgent>().gameObject;
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
                //Debug.Log(transform.root.name + " STABBED " + collision.transform.root.name);
                AttackedHealth.TakeDamage(MeleeConfig.DamageType, MeleeConfig.Damage);
            }
        }
        
    }
}
