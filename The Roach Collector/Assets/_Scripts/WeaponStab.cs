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


    private void Start()
    {
        _player = FindObjectOfType<PlayerController>().gameObject;

        if (transform.root.name == "Core") _wsCheck = _player.GetComponent<WeaponStabCheck>();
        else _wsCheck = GetComponentInParent<WeaponStabCheck>();

    }


    private void OnCollisionEnter(Collision collision)
    {
        //are we stabbing?
        _isStabbing = _wsCheck.GetStabbing();

        //See if we are stabbing an agent
        AIAgent agent = collision.transform.GetComponentInParent<AIAgent>();

        //if we are then the parent is the agent
        if (agent)
        {
            _parent = agent.gameObject;
            _attackedHealth = _parent.GetComponent<Health>();
        }
        else
        {
            _parent = _player;
            _attackedHealth = _player.GetComponent<Health>();
        }

        Debug.Log(_wsCheck.gameObject.name);

        if (collision.transform.root != transform.root)
        {
            Debug.Log("Boop");
            //Get the health of the attacked object and deal damage
            if (_isStabbing)
            {
                Debug.Log("Beep");
                _attackedHealth.TakeDamage(_meleeConfig.DamageType, _meleeConfig.Damage);
            }
        }

    }
}
