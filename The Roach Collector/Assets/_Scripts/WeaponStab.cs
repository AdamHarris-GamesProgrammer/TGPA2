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
        //are we stabbing?
        _isStabbing = _wsCheck.GetStabbing();

        //See if we are stabbing an agent
        AIAgent agent = collision.transform.GetComponentInParent<AIAgent>();

        //if we are then the parent is the agent
        if (agent) _parent = agent.gameObject;
        //if not then we are stabbing a player
        else _parent = _player;

        if (collision.transform.root != transform.root)
        {
            //Get the health of the attacked object and deal damage
            if (_isStabbing) _parent.GetComponent<Health>().TakeDamage(_meleeConfig.DamageType, _meleeConfig.Damage);
        }
        
    }
}
