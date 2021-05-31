using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class WeaponStab : MonoBehaviour
{
    private Health _attackedHealth;
    private GameObject _parent;
    private GameObject _player;

    private WeaponStabCheck _wsCheck;
    private bool _isStabbing = false;
    [SerializeField] WeaponConfig _meleeConfig;


    private void Awake()
    {
        _player =  FindObjectOfType<PlayerController>().gameObject;

        AIAgent agent = GetComponentInParent<AIAgent>();
        if (agent) _wsCheck = agent.GetComponent<WeaponStabCheck>();
        else _wsCheck = _player.GetComponent<WeaponStabCheck>();

    }


    private void OnCollisionEnter(Collision collision)
    {
        _isStabbing = _wsCheck.GetStabbing();

        AIAgent agent = collision.transform.GetComponentInParent<AIAgent>();
        if (agent) _parent = agent.gameObject;
        else _parent = _player;

        if (collision.transform.root != transform.root)
        {
            if (_parent == _player) _attackedHealth = _parent.GetComponent<PlayerHealth>();
            else _attackedHealth = _parent.GetComponent<AIHealth>();

            if (_isStabbing) _attackedHealth.TakeDamage(_meleeConfig.DamageType, _meleeConfig.Damage);
        }
        
    }
}
